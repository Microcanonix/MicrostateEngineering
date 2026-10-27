import { ChangeDetectionStrategy, Component, computed, effect, inject, input } from '@angular/core';
import { MoleculeReportData, parseMoleculeReport } from '../models/molecule-report.model';
import {
  ATOM_COLOR_MODES,
  AoChartMode,
  AtomColorMode,
  BondColorMode,
  MoleculeReportExplorerState,
} from '../services/molecule-report-explorer-state.service';
import { MoleculeViewer } from './molecule-viewer';
import { AoConfigChart } from './ao-config-chart';
import { GroupedBars, GroupedBarSeries } from './grouped-bars';

@Component({
  selector: 'app-molecule-report-explorer',
  templateUrl: './molecule-report-explorer.html',
  styleUrl: './molecule-report-explorer.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [MoleculeViewer, AoConfigChart, GroupedBars],
  providers: [MoleculeReportExplorerState],
})
export class MoleculeReportExplorer {
  private readonly state = inject(MoleculeReportExplorerState);

  /** Raw report object: PascalCase (file convention) or camelCase (WebAPI). */
  readonly report = input.required<unknown>();

  readonly atomColorModes = ATOM_COLOR_MODES;

  readonly atoms = this.state.atoms;
  readonly bonds = this.state.bonds;
  readonly moleculeName = this.state.moleculeName;
  readonly selectedAtomId = this.state.selectedAtomId;
  readonly hoveredAtomId = this.state.hoveredAtomId;
  readonly hoveredBondId = this.state.hoveredBondId;
  readonly aoChartAtom = this.state.aoChartAtom;
  readonly verificationChips = computed(() => this.state.verification().chips);

  readonly atomColorMode = this.state.atomColorMode;
  readonly bondColorMode = this.state.bondColorMode;
  readonly aoChartMode = this.state.aoChartMode;

  constructor() {
    effect(() => {
      const normalized = parseMoleculeReport(this.report());
      this.state.setReport(normalized);
    });
  }

  onAtomHovered(id: string | null): void {
    this.state.hoveredAtomId.set(id);
  }

  onAtomClicked(id: string): void {
    this.state.selectAtom(id);
  }

  onBondHovered(id: string | null): void {
    this.state.hoveredBondId.set(id);
  }

  onAtomRowHover(atomId: string | null): void {
    this.state.hoveredAtomId.set(atomId);
  }

  onAtomRowLeave(): void {
    this.state.hoveredAtomId.set(null);
  }

  onAtomRowClick(atomId: string): void {
    this.state.selectAtom(atomId);
  }

  setAtomColorMode(mode: AtomColorMode): void {
    this.state.atomColorMode.set(mode);
  }

  setBondColorMode(mode: BondColorMode): void {
    this.state.bondColorMode.set(mode);
  }

  setAoChartMode(mode: AoChartMode): void {
    this.state.aoChartMode.set(mode);
  }

  chargeClass(value: number): string {
    if (value > 0) {
      return 'text-primary';
    }
    if (value < 0) {
      return 'text-danger';
    }
    return '';
  }

  frontierClass(value: number): string {
    if (value > 0) {
      return 'mre-positive';
    }
    if (value < 0) {
      return 'mre-negative';
    }
    return '';
  }

  readonly chargeSeries = computed<GroupedBarSeries[]>(() => {
    const atoms = this.atoms();
    return [
      { label: 'Mulliken', color: '#4169e1', values: atoms.map((a) => a.charge?.MullikenCharge ?? 0) },
      { label: 'Lowdin', color: '#7c9ef0', values: atoms.map((a) => a.charge?.LowdinCharge ?? 0) },
      { label: 'CHelpG(H)', color: '#d98a2b', values: atoms.map((a) => a.charge?.CHelpGHCharge ?? 0) },
      { label: 'GeoDisc', color: '#d64545', values: atoms.map((a) => a.charge?.GeoDiscCharge ?? 0) },
    ];
  });

  readonly shareSeries = computed<GroupedBarSeries[]>(() => {
    const atoms = this.atoms();
    return [
      { label: 'HOMO (Mulliken)', color: '#4169e1', values: atoms.map((a) => a.population?.MullikenPopulationHOMO ?? 0) },
      { label: 'LUMO (Mulliken)', color: '#d64545', values: atoms.map((a) => a.population?.MullikenPopulationLUMO ?? 0) },
      { label: 'HOMO (Lowdin)', color: '#7c9ef0', values: atoms.map((a) => a.population?.LowdinPopulationHOMO ?? 0) },
      { label: 'LUMO (Lowdin)', color: '#f0a08a', values: atoms.map((a) => a.population?.LowdinPopulationLUMO ?? 0) },
    ];
  });

  readonly atomCategories = computed(() => this.atoms().map((a) => a.id));
}
