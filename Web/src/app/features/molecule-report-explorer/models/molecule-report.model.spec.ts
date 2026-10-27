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

  it('returns null for objects without the expected shape', () => {
    expect(parseMoleculeReport(null)).toBeNull();
    expect(parseMoleculeReport({ unrelated: [] })).toBeNull();
    expect(parseMoleculeReport({ AtomPositionsReport: [] })).toBeNull();
  });
});
