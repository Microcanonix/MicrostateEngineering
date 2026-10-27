import { computed, Injectable, signal } from '@angular/core';
import {
  AoPopulation,
  AtomChargeReport,
  AtomOrbitalReport,
  AtomPopulationReport,
  AtomPositionReport,
  MoleculeBondReport,
  MoleculeReportData,
  parseAoConfiguration,
} from '../models/molecule-report.model';

export type AtomColorMode =
  | 'elements'
  | 'mulliken'
  | 'lowdin'
  | 'chelpg'
  | 'geodisc'
  | 'homo-mulliken'
  | 'lumo-mulliken'
  | 'homo-lowdin'
  | 'lumo-lowdin';

export const ATOM_COLOR_MODES: { value: AtomColorMode; label: string }[] = [
  { value: 'elements', label: 'Element colours' },
  { value: 'mulliken', label: 'Mulliken charge' },
  { value: 'lowdin', label: 'Lowdin charge' },
  { value: 'chelpg', label: 'CHelpG(H) charge' },
  { value: 'geodisc', label: 'GeoDisc charge' },
  { value: 'homo-mulliken', label: 'HOMO share (Mulliken)' },
  { value: 'lumo-mulliken', label: 'LUMO share (Mulliken)' },
  { value: 'homo-lowdin', label: 'HOMO share (Lowdin)' },
  { value: 'lumo-lowdin', label: 'LUMO share (Lowdin)' },
];

export type BondColorMode = 'none' | 'homo' | 'lumo';

export type AoChartMode = 'ground' | 'fractions' | 'homo' | 'lumo';

export interface AtomView {
  position: AtomPositionReport;
  charge?: AtomChargeReport;
  population?: AtomPopulationReport;
  orbital?: AtomOrbitalReport;
  /** 1-based position, matches AtomPosition and Atom1Pos/Atom2Pos. */
  pos: number;
  id: string;
  symbol: string;
}

export interface VerificationChip {
  label: string;
  value: string;
  pass: boolean;
}

const ELEMENT_COLORS: Record<string, number> = {
  H: 0xe8e8e8,
  C: 0x475569,
  O: 0xd64545,
};

export interface VerificationResult {
  chips: VerificationChip[];
}

@Injectable()
export class MoleculeReportExplorerState {
  private readonly report = signal<MoleculeReportData | null>(null);

  readonly reportInput = this.report.asReadonly();

  readonly atoms = computed<AtomView[]>(() => {
    const report = this.report();
    if (!report) {
      return [];
    }

    const charges = new Map(report.AtomChargeReport.map((c) => [c.AtomID, c]));
    const populations = new Map(report.AtomPopulationReport.map((p) => [p.AtomID, p]));
    const orbitals = new Map(report.AtomOrbitalReport.map((o) => [o.AtomID, o]));

    return report.AtomPositionsReport.map((position) => {
      const id = `${position.AtomSymbol}${position.AtomPosition}`;
      return {
        position,
        charge: charges.get(id),
        population: populations.get(id),
        orbital: orbitals.get(id),
        pos: position.AtomPosition,
        id,
        symbol: position.AtomSymbol,
      };
    });
  });

  readonly bonds = computed<MoleculeBondReport[]>(() => this.report()?.MoleculeBondsReport ?? []);

  readonly moleculeName = computed(() => this.report()?.AtomPositionsReport[0]?.MoleculeName ?? '');

  readonly atomColorMode = signal<AtomColorMode>('elements');
  readonly bondColorMode = signal<BondColorMode>('none');
  readonly aoChartMode = signal<AoChartMode>('ground');

  readonly selectedAtomId = signal<string | null>(null);
  readonly hoveredAtomId = signal<string | null>(null);
  readonly hoveredBondId = signal<string | null>(null);

  /** Atom shown in the AO chart: selected atom, else hovered atom. */
  readonly aoChartAtom = computed<AtomView | null>(() => {
    const selected = this.selectedAtomId();
    if (selected) {
      return this.atoms().find((a) => a.id === selected) ?? null;
    }
    const hovered = this.hoveredAtomId();
    if (hovered) {
      return this.atoms().find((a) => a.id === hovered) ?? null;
    }
    return null;
  });

  readonly verification = computed<VerificationResult>(() => this.computeVerification());

  setReport(report: MoleculeReportData | null): void {
    this.report.set(report);
    this.selectedAtomId.set(null);
    this.hoveredAtomId.set(null);
    this.hoveredBondId.set(null);
  }

  selectAtom(id: string): void {
    this.selectedAtomId.set(this.selectedAtomId() === id ? null : id);
  }

  private computeVerification(): VerificationResult {
    const report = this.report();
    if (!report) {
      return { chips: [] };
    }

    const chips: VerificationChip[] = [];
    const tol = 0.005;
    const fmt = (v: number) => v.toFixed(4);

    const chargeSchemes: { key: keyof AtomChargeReport; label: string }[] = [
      { key: 'MullikenCharge', label: 'Mulliken' },
      { key: 'LowdinCharge', label: 'Lowdin' },
      { key: 'CHelpGHCharge', label: 'CHelpG(H)' },
      { key: 'GeoDiscCharge', label: 'GeoDisc' },
    ];
    for (const scheme of chargeSchemes) {
      const sum = report.AtomChargeReport.reduce((acc, c) => acc + (c[scheme.key] as number), 0);
      chips.push({
        label: `Σ ${scheme.label} charge`,
        value: fmt(sum),
        pass: Math.abs(sum) <= tol,
      });
    }

    const shareSchemes: { homo: keyof AtomPopulationReport; lumo: keyof AtomPopulationReport; label: string }[] = [
      { homo: 'MullikenPopulationHOMO', lumo: 'MullikenPopulationLUMO', label: 'Mulliken' },
      { homo: 'LowdinPopulationHOMO', lumo: 'LowdinPopulationLUMO', label: 'Lowdin' },
    ];
    for (const scheme of shareSchemes) {
      const homoSum = report.AtomPopulationReport.reduce((acc, p) => acc + (p[scheme.homo] as number), 0);
      const lumoSum = report.AtomPopulationReport.reduce((acc, p) => acc + (p[scheme.lumo] as number), 0);
      chips.push({
        label: `Σ ${scheme.label} HOMO shares`,
        value: fmt(homoSum),
        pass: Math.abs(homoSum - 1) <= tol,
      });
      chips.push({
        label: `Σ ${scheme.label} LUMO shares`,
        value: fmt(lumoSum),
        pass: Math.abs(lumoSum - 1) <= tol,
      });
    }

    const positions = new Map(report.AtomPositionsReport.map((p) => [p.AtomPosition, p]));
    const atomicNumbers: Record<string, number> = { H: 1, C: 6, N: 7, O: 8 };
    const neutrality = (populationKey: 'MullikenPopulation' | 'LowdinPopulation', label: string): void => {
      let maxDeviation = 0;
      let worst = '';
      for (const charge of report.AtomChargeReport) {
        const population = report.AtomPopulationReport.find((p) => p.AtomID === charge.AtomID);
        if (!population) {
          continue;
        }
        const position = positions.get(atomPositionFromId(charge.AtomID));
        const z = position ? (atomicNumbers[position.AtomSymbol] ?? 0) : 0;
        const deviation = Math.abs((population[populationKey] as number) + (charge[schemeKey(populationKey)] as number) - z);
        if (deviation > maxDeviation) {
          maxDeviation = deviation;
          worst = charge.AtomID;
        }
      }
      chips.push({
        label: `max |population + charge − Z| (${label})`,
        value: `${fmt(maxDeviation)}${worst ? ` (${worst})` : ''}`,
        pass: maxDeviation <= 1e-6,
      });
    };

    const schemeKey = (populationKey: 'MullikenPopulation' | 'LowdinPopulation'): keyof AtomChargeReport =>
      populationKey === 'MullikenPopulation' ? 'MullikenCharge' : 'LowdinCharge';

    neutrality('MullikenPopulation', 'Mulliken');
    neutrality('LowdinPopulation', 'Lowdin');

    const configCheck = (
      configKey: 'ElectronConfiguration' | 'ElectronConfigurationHOMO' | 'ElectronConfigurationLUMO',
      targetKey: keyof AtomPopulationReport,
      label: string,
    ): void => {
      let maxDeviation = 0;
      for (const orbital of report.AtomOrbitalReport) {
        const populations = report.AtomPopulationReport.find((p) => p.AtomID === orbital.AtomID);
        if (!populations) {
          continue;
        }
        const values = parseAoConfiguration(orbital[configKey] as string);
        const sum = values.reduce((acc, v) => acc + v.value, 0);
        const deviation = Math.abs(sum - (populations[targetKey] as number));
        if (deviation > maxDeviation) {
          maxDeviation = deviation;
        }
      }
      chips.push({
        label: `max |Σ AO − ${label}|`,
        value: fmt(maxDeviation),
        pass: maxDeviation <= 0.0005,
      });
    };

    configCheck('ElectronConfiguration', 'MullikenPopulation', 'population');
    configCheck('ElectronConfigurationHOMO', 'MullikenPopulationHOMO', 'HOMO share');
    configCheck('ElectronConfigurationLUMO', 'MullikenPopulationLUMO', 'LUMO share');

    let maxFractionDeviation = 0;
    for (const orbital of report.AtomOrbitalReport) {
      const populations = report.AtomPopulationReport.find((p) => p.AtomID === orbital.AtomID);
      if (!populations) {
        continue;
      }
      const values = parseAoConfiguration(orbital.ElectronFractionConfiguration);
      const sum = values.reduce((acc, v) => acc + v.value, 0);
      const deviation = Math.abs(sum - 1);
      if (deviation > maxFractionDeviation) {
        maxFractionDeviation = deviation;
      }
    }
    chips.push({
      label: 'max |Σ ground fractions − 1|',
      value: fmt(maxFractionDeviation),
      pass: maxFractionDeviation <= 0.0005,
    });

    return { chips };
  }
}

function atomPositionFromId(atomId: string): number {
  const match = atomId.match(/(?:\d+)$/);
  return match ? Number(match[0]) : -1;
}

export function atomPropertyValue(atom: AtomView, mode: AtomColorMode): number | null {
  if (!atom.charge || !atom.population) {
    return null;
  }
  switch (mode) {
    case 'mulliken':
      return atom.charge.MullikenCharge;
    case 'lowdin':
      return atom.charge.LowdinCharge;
    case 'chelpg':
      return atom.charge.CHelpGHCharge;
    case 'geodisc':
      return atom.charge.GeoDiscCharge;
    case 'homo-mulliken':
      return atom.population.MullikenPopulationHOMO;
    case 'lumo-mulliken':
      return atom.population.MullikenPopulationLUMO;
    case 'homo-lowdin':
      return atom.population.LowdinPopulationHOMO;
    case 'lumo-lowdin':
      return atom.population.LowdinPopulationLUMO;
    default:
      return null;
  }
}

export function elementColor(symbol: string): number {
  return ELEMENT_COLORS[symbol] ?? 0x888888;
}

export function divergingColor(value: number, max: number): [number, number, number] {
  if (max <= 0) {
    return [255, 255, 255];
  }
  const t = Math.max(-1, Math.min(1, value / max));
  if (t >= 0) {
    return [Math.round(255 * (1 - t)), Math.round(255 * (1 - t)), 255];
  }
  return [255, Math.round(255 * (1 + t)), Math.round(255 * (1 + t))];
}

export function aoValues(orbital: AtomOrbitalReport, mode: AoChartMode): AoPopulation[] {
  switch (mode) {
    case 'ground':
      return parseAoConfiguration(orbital.ElectronConfiguration);
    case 'fractions':
      return parseAoConfiguration(orbital.ElectronFractionConfiguration);
    case 'homo':
      return parseAoConfiguration(orbital.ElectronConfigurationHOMO);
    case 'lumo':
      return parseAoConfiguration(orbital.ElectronConfigurationLUMO);
  }
}
