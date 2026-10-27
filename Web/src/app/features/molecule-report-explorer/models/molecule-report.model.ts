export interface AtomPositionReport {
  MoleculeName: string;
  AtomPosition: number;
  AtomSymbol: string;
  PosX: number;
  PosY: number;
  PosZ: number;
}

export interface AtomChargeReport {
  MoleculeName: string;
  AtomID: string;
  MullikenCharge: number;
  LowdinCharge: number;
  CHelpGHCharge: number;
  GeoDiscCharge: number;
}

export interface AtomPopulationReport {
  AtomID: string;
  MullikenPopulation: number;
  MullikenPopulationHOMO: number;
  MullikenPopulationLUMO: number;
  LowdinPopulation: number;
  LowdinPopulationHOMO: number;
  LowdinPopulationLUMO: number;
}

export interface AtomOrbitalReport {
  AtomID: string;
  MullikenPopulation: number;
  ElectronFractionConfiguration: string;
  ElectronConfiguration: string;
  ElectronConfigurationHOMO: string;
  ElectronConfigurationLUMO: string;
}

export interface MoleculeBondReport {
  MoleculeName: string;
  Atom1Pos: number;
  Atom2Pos: number;
  BondID: string;
  Distance: number;
  BondOrder: number;
  OverlapPopulation: number;
  OverlapPopulationHOMO: number;
  OverlapPopulationLUMO: number;
}

export interface MoleculeReportData {
  AtomPositionsReport: AtomPositionReport[];
  AtomChargeReport: AtomChargeReport[];
  AtomPopulationReport: AtomPopulationReport[];
  AtomOrbitalReport: AtomOrbitalReport[];
  MoleculeBondsReport: MoleculeBondReport[];
}

const SECTION_KEYS: Record<string, { pascal: string; camel: string }> = {
  AtomPositionsReport: { pascal: 'AtomPositionsReport', camel: 'atomPositionsReport' },
  AtomChargeReport: { pascal: 'AtomChargeReport', camel: 'atomChargeReport' },
  AtomPopulationReport: { pascal: 'AtomPopulationReport', camel: 'atomPopulationReport' },
  AtomOrbitalReport: { pascal: 'AtomOrbitalReport', camel: 'atomOrbitalReport' },
  MoleculeBondsReport: { pascal: 'MoleculeBondsReport', camel: 'moleculeBondsReport' },
};

/**
 * Accepts a raw report object with either PascalCase (report file convention)
 * or camelCase (ASP.NET Core JSON serialization) section keys and returns a
 * normalized MoleculeReportData, or null if the object is not of this shape.
 */
export function parseMoleculeReport(raw: unknown): MoleculeReportData | null {
  if (!raw || typeof raw !== 'object') {
    return null;
  }

  const source = raw as Record<string, unknown>;
  const sections: Record<string, unknown[]> = {};

  for (const [canonical, keys] of Object.entries(SECTION_KEYS)) {
    const section = source[keys.pascal] ?? source[keys.camel];
    if (!Array.isArray(section)) {
      return null;
    }
    sections[canonical] = section;
  }

  return {
    AtomPositionsReport: sections['AtomPositionsReport'] as AtomPositionReport[],
    AtomChargeReport: sections['AtomChargeReport'] as AtomChargeReport[],
    AtomPopulationReport: sections['AtomPopulationReport'] as AtomPopulationReport[],
    AtomOrbitalReport: sections['AtomOrbitalReport'] as AtomOrbitalReport[],
    MoleculeBondsReport: sections['MoleculeBondsReport'] as MoleculeBondReport[],
  };
}

export interface AoPopulation {
  label: string;
  value: number;
}

/**
 * Parses an AO configuration string such as
 * "S(1.9883)S(0.3545)X(0.5410)Y(0.5442)Z(0.5397)S(1.1265)X(0.4690)Y(0.5285)Z(0.5015)"
 * into per-basis-function populations in basis order. Values are raw and
 * unnormalized; negative values are kept exactly as given.
 */
export function parseAoConfiguration(configuration: string | null | undefined): AoPopulation[] {
  if (!configuration) {
    return [];
  }

  const result: AoPopulation[] = [];
  const pattern = /([A-Za-z])\((-?\d+(?:\.\d+)?(?:[eE][+-]?\d+)?)\)/g;
  let match: RegExpExecArray | null;
  while ((match = pattern.exec(configuration)) !== null) {
    result.push({ label: match[1], value: Number.parseFloat(match[2]) });
  }

  return result;
}

export const ATOMIC_NUMBERS: Record<string, number> = {
  H: 1,
  C: 6,
  N: 7,
  O: 8,
};

export const HEAVY_ATOM_BASIS_ORDER = 'S S X Y Z S X Y Z';
export const HYDROGEN_BASIS_ORDER = 'S S';
