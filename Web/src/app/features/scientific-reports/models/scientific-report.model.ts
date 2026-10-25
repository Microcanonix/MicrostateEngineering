export interface MoleculeScientificReport {
  moleculeName: string;
  report?: unknown;
}

export interface ResearchDefinitionScientificReport {
  researchDefinitionName: string;
  moleculeReports: MoleculeScientificReport[];
}
