import { describe, expect, it } from 'vitest';
import { parseAoConfiguration, parseMoleculeReport } from './molecule-report.model';

describe('parseAoConfiguration', () => {
  it('parses raw unnormalized values in basis order', () => {
    const values = parseAoConfiguration(
      'S(1.9883)S(0.3545)X(0.5410)Y(0.5442)Z(0.5397)S(1.1265)X(0.4690)Y(0.5285)Z(0.5015)',
    );
    expect(values.length).toBe(9);
    expect(values[0]).toEqual({ label: 'S', value: 1.9883 });
    expect(values[8]).toEqual({ label: 'Z', value: 0.5015 });
  });

  it('keeps negative values exactly as given', () => {
    const values = parseAoConfiguration('S(0.0000)S(0.0019)X(-0.0035)');
    expect(values[2]).toEqual({ label: 'X', value: -0.0035 });
  });

  it('returns an empty array for missing input', () => {
    expect(parseAoConfiguration(undefined)).toEqual([]);
    expect(parseAoConfiguration(null)).toEqual([]);
    expect(parseAoConfiguration('')).toEqual([]);
  });
});

describe('parseMoleculeReport', () => {
  it('accepts PascalCase section keys', () => {
    const report = parseMoleculeReport({
      AtomPositionsReport: [],
      AtomChargeReport: [],
      AtomPopulationReport: [],
      AtomOrbitalReport: [],
      MoleculeBondsReport: [],
    });
    expect(report).not.toBeNull();
  });

  it('accepts camelCase section keys', () => {
    const report = parseMoleculeReport({
      atomPositionsReport: [],
      atomChargeReport: [],
      atomPopulationReport: [],
      atomOrbitalReport: [],
      moleculeBondsReport: [],
    });
    expect(report).not.toBeNull();
  });

  it('maps camelCase record fields onto the PascalCase convention', () => {
    const report = parseMoleculeReport({
      AtomPositionsReport: [
        { moleculeName: 'ethanol', atomPosition: 1, atomSymbol: 'C', posX: 0.1, posY: 0.2, posZ: 0.3 },
      ],
      AtomChargeReport: [
        { moleculeName: 'ethanol', atomID: 'C1', mullikenCharge: -0.5, lowdinCharge: -0.2, chelpGHCharge: -0.3, geoDiscCharge: -0.3 },
      ],
      AtomPopulationReport: [
        { atomID: 'C1', mullikenPopulation: 6.5, mullikenPopulationHOMO: 0.1, mullikenPopulationLUMO: 0.2, lowdinPopulation: 6.2, lowdinPopulationHOMO: 0.05, lowdinPopulationLUMO: 0.1 },
      ],
      AtomOrbitalReport: [
        { atomID: 'C1', mullikenPopulation: 6.5, electronConfiguration: 'S(1.0)', electronConfigurationHOMO: 'S(0.1)', electronConfigurationLUMO: 'S(0.2)', electronFractionConfiguration: 'S(1.0)' },
      ],
      MoleculeBondsReport: [
        { moleculeName: 'ethanol', atom1Pos: 1, atom2Pos: 2, bondID: 'C1-H2', distance: 1.09, bondOrder: 0.9, overlapPopulation: 0.37, overlapPopulationHOMO: 0.01, overlapPopulationLUMO: -0.004 },
      ],
    });

    expect(report).not.toBeNull();
    expect(report!.AtomPositionsReport[0].AtomSymbol).toBe('C');
    expect(report!.AtomPositionsReport[0].AtomPosition).toBe(1);
    expect(report!.AtomChargeReport[0].AtomID).toBe('C1');
    expect(report!.AtomChargeReport[0].MullikenCharge).toBe(-0.5);
    expect(report!.AtomChargeReport[0].CHelpGHCharge).toBe(-0.3);
    expect(report!.AtomPopulationReport[0].MullikenPopulationHOMO).toBe(0.1);
    expect(report!.AtomOrbitalReport[0].ElectronConfiguration).toBe('S(1.0)');
    expect(report!.MoleculeBondsReport[0].BondID).toBe('C1-H2');
    expect(report!.MoleculeBondsReport[0].Atom1Pos).toBe(1);
    expect(report!.MoleculeBondsReport[0].OverlapPopulationLUMO).toBe(-0.004);
  });

  it('returns null for objects without the expected shape', () => {
    expect(parseMoleculeReport(null)).toBeNull();
    expect(parseMoleculeReport({ unrelated: [] })).toBeNull();
    expect(parseMoleculeReport({ AtomPositionsReport: [] })).toBeNull();
  });
});
