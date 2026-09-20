export enum ProcessType {
  Dummy = 0,
  MoleculeProperties = 1,
}

export enum StepType {
  Dummy = 0,
  ImportData = 1,
  GeometryOptimization = 2,
  ElectronicStructure = 3,
  FukuiCalculation = 4,
  ChargeGeodisk = 5,
  ChargeChelpg = 6,
}

export interface ResearchDefinitionMolecule {
  name: string;
  charge: number;
}

export interface ResearchDefinitionProcessStep {
  id: number;
  type: StepType;
  canExecute: boolean;
}

export interface ResearchDefinitionProcessDependency {
  dependency: StepType;
  dependant: StepType;
}

export interface ResearchDefinitionProcess {
  type: ProcessType;
  steps: ResearchDefinitionProcessStep[];
  dependencies: ResearchDefinitionProcessDependency[];
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
  molecules: ResearchDefinitionMolecule[];
  processes: ResearchDefinitionProcess[];
}

export const PROCESS_TYPE_OPTIONS = [
  { value: ProcessType.Dummy, label: 'dummy' },
  { value: ProcessType.MoleculeProperties, label: 'moleculeproperties' },
] as const;

export const STEP_TYPE_OPTIONS = [
  { value: StepType.Dummy, label: 'dummy' },
  { value: StepType.ImportData, label: 'import_data' },
  { value: StepType.GeometryOptimization, label: 'geometry_optimization' },
  { value: StepType.ElectronicStructure, label: 'electronic_structure' },
  { value: StepType.FukuiCalculation, label: 'fukui_calculation' },
  { value: StepType.ChargeGeodisk, label: 'charge_geodisk' },
  { value: StepType.ChargeChelpg, label: 'charge_chelpg' },
] as const;

export function processTypeLabel(type: ProcessType): string {
  return PROCESS_TYPE_OPTIONS.find((option) => option.value === type)?.label ?? String(type);
}

export function createResearchDefinition(): ResearchDefinition {
  return {
    name: '',
    packageRoot: 'C:\\MoleculesDb',
    xyzfiles: 'xyz',
    gmsInput: 'input',
    gmsOutput: 'output',
    workflowStatusFolder: 'status',
    moleculeData: 'molecules',
    basisset: 'B3_21G',
    molecules: [],
    processes: [
      {
        type: ProcessType.MoleculeProperties,
        steps: [
          { id: 0, type: StepType.ImportData, canExecute: true },
          { id: 1, type: StepType.GeometryOptimization, canExecute: true },
          { id: 2, type: StepType.ElectronicStructure, canExecute: true },
          { id: 3, type: StepType.FukuiCalculation, canExecute: true },
          { id: 4, type: StepType.ChargeGeodisk, canExecute: true },
          { id: 5, type: StepType.ChargeChelpg, canExecute: true },
        ],
        dependencies: [
          { dependency: StepType.ImportData, dependant: StepType.GeometryOptimization },
          { dependency: StepType.GeometryOptimization, dependant: StepType.ElectronicStructure },
          { dependency: StepType.GeometryOptimization, dependant: StepType.FukuiCalculation },
          { dependency: StepType.GeometryOptimization, dependant: StepType.ChargeGeodisk },
          { dependency: StepType.GeometryOptimization, dependant: StepType.ChargeChelpg },
        ],
      },
    ],
  };
}
