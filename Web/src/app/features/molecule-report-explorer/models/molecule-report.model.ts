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
  /** Null when GAMESS omits the bond from its bond-order printout. */
  Distance: number | null;
  BondOrder: number | null;
  OverlapPopulation: number | null;
  OverlapPopulationHOMO: number | null;
  OverlapPopulationLUMO: number | null;
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

/** Field names per section, in the PascalCase report-file convention. */
const SECTION_FIELDS: Record<string, string[]> = {
  AtomPositionsReport: ['MoleculeName', 'AtomPosition', 'AtomSymbol', 'PosX', 'PosY', 'PosZ'],
  AtomChargeReport: ['MoleculeName', 'AtomID', 'MullikenCharge', 'LowdinCharge', 'CHelpGHCharge', 'GeoDiscCharge'],
  AtomPopulationReport: [
    'AtomID',
    'MullikenPopulation',
    'MullikenPopulationHOMO',
    'MullikenPopulationLUMO',
    'LowdinPopulation',
    'LowdinPopulationHOMO',
    'LowdinPopulationLUMO',
  ],
  AtomOrbitalReport: [
    'AtomID',
    'MullikenPopulation',
    'ElectronFractionConfiguration',
    'ElectronConfiguration',
    'ElectronConfigurationHOMO',
    'ElectronConfigurationLUMO',
  ],
  MoleculeBondsReport: [
    'MoleculeName',
    'Atom1Pos',
    'Atom2Pos',
    'BondID',
    'Distance',
    'BondOrder',
    'OverlapPopulation',
    'OverlapPopulationHOMO',
    'OverlapPopulationLUMO',
  ],
};

/**
 * Normalizes a section of raw records: keeps objects that already use the
 * PascalCase convention untouched and maps camelCase records (ASP.NET Core
 * JSON serialization, e.g. "atomID", "chelpGHCharge") onto the PascalCase
 * field names. Matching is case-insensitive so any serializer convention
 * maps correctly.
 */
function normalizeSection<T>(section: unknown[], fields: string[]): T[] {
  return section.map((record) => {
    if (!record || typeof record !== 'object') {
      return record as T;
    }

    const source = record as Record<string, unknown>;
    // lowercase key -> original key in the raw record
    const byLowercase = new Map<string, string>();
    for (const key of Object.keys(source)) {
      if (!byLowercase.has(key.toLowerCase())) {
        byLowercase.set(key.toLowerCase(), key);
      }
    }

    const normalized: Record<string, unknown> = {};
    for (const field of fields) {
      const originalKey = byLowercase.get(field.toLowerCase());
      if (originalKey !== undefined) {
        normalized[field] = source[originalKey];
      }
    }
    // Preserve any fields not explicitly listed.
    for (const [key, value] of Object.entries(source)) {
      if (!(key.toLowerCase() in normalized) && !fields.some((f) => f.toLowerCase() === key.toLowerCase())) {
        normalized[key] = value;
      }
    }
    return normalized as T;
  });
}

/**
 * Accepts a raw report object with either PascalCase (report file convention)
 * or camelCase (ASP.NET Core JSON serialization) section and field names and
 * returns a normalized MoleculeReportData, or null if the object is not of
 * this shape.
 */
export function parseMoleculeReport(raw: unknown): MoleculeReportData | null {
  if (!raw || typeof raw !== 'object') {
    return null;
  }

  const source = raw as Record<string, unknown>;

  const readSection = (canonical: string): unknown[] | null => {
    const keys = SECTION_KEYS[canonical];
    const section = source[keys.pascal] ?? source[keys.camel];
    return Array.isArray(section) ? section : null;
  };

  const positions = readSection('AtomPositionsReport');
  const charges = readSection('AtomChargeReport');
  const populations = readSection('AtomPopulationReport');
  const orbitals = readSection('AtomOrbitalReport');
  const bonds = readSection('MoleculeBondsReport');

  if (!positions || !charges || !populations || !orbitals || !bonds) {
    return null;
  }

  return {
    AtomPositionsReport: normalizeSection<AtomPositionReport>(
      positions,
      SECTION_FIELDS['AtomPositionsReport'],
    ),
    AtomChargeReport: normalizeSection<AtomChargeReport>(
      charges,
      SECTION_FIELDS['AtomChargeReport'],
    ),
    AtomPopulationReport: normalizeSection<AtomPopulationReport>(
      populations,
      SECTION_FIELDS['AtomPopulationReport'],
    ),
    AtomOrbitalReport: normalizeSection<AtomOrbitalReport>(
      orbitals,
      SECTION_FIELDS['AtomOrbitalReport'],
    ),
    MoleculeBondsReport: normalizeSection<MoleculeBondReport>(
      bonds,
      SECTION_FIELDS['MoleculeBondsReport'],
    ),
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
