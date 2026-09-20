export interface ResearchDefinitionProcess {
  type: number;
}

export interface ResearchDefinition {
  name: string;
  packageRoot: string;
  xyzfiles: string;
  gmsInput: string;
  gmsOutput: string;
  workflowStatusFolder: string;
  moleculeData: string;
  basisset: string;
  molecules: unknown[];
  processes: ResearchDefinitionProcess[];
}

export interface WorkflowSummary {
  id: string;
  name: string;
  basisSet: string;
  packageRoot: string;
  processTypes: string[];
}

export interface WorkflowDocument extends WorkflowSummary {
  yaml: string;
}
