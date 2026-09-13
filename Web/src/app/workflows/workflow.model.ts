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
