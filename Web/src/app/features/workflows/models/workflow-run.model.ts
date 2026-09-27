export interface MoleculeResult {
  moleculeName: string;
  succeeded: boolean;
}

export interface WorkflowRunReport {
  name: string;
  moleculeResult: MoleculeResult[];
}
