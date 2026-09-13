import { Injectable } from '@angular/core';
import { WorkflowDocument, WorkflowSummary } from './workflow.model';
import { STARTER_WORKFLOW_YAML } from './workflow-template';

const STORAGE_KEY = 'microstate-engineering.workflows.v1';

const INITIAL_WORKFLOWS: WorkflowDocument[] = [
  {
    id: 'alcohol',
    name: 'alcohol',
    basisSet: 'B3_21G',
    packageRoot: 'C:\\MoleculesDb',
    processTypes: ['moleculeproperties'],
    yaml: STARTER_WORKFLOW_YAML.replace('new-workflow', 'alcohol').replace('example-molecule', 'ethanol'),
  },
  {
    id: 'aminoacidsidechain',
    name: 'aminoacidsidechain',
    basisSet: 'B3_21G',
    packageRoot: 'C:\\MoleculesDb',
    processTypes: ['moleculeproperties'],
    yaml: STARTER_WORKFLOW_YAML.replace('new-workflow', 'aminoacidsidechain').replace('example-molecule', 'alanine-reference'),
  },
  {
    id: 'medicalplants',
    name: 'medicalplants',
    basisSet: 'B3_21G',
    packageRoot: 'C:\\MoleculesDb',
    processTypes: ['moleculeproperties'],
    yaml: STARTER_WORKFLOW_YAML.replace('new-workflow', 'medicalplants').replace('example-molecule', 'absinthin'),
  },
  {
    id: 'medicalplants-2nd',
    name: 'medicalplants-2nd',
    basisSet: 'B6_31G',
    packageRoot: 'C:\\MoleculesDb',
    processTypes: ['moleculeproperties'],
    yaml: STARTER_WORKFLOW_YAML.replace('new-workflow', 'medicalplants-2nd').replace('B3_21G', 'B6_31G').replace('example-molecule', 'alpha-d-glucopyranose'),
  },
];

@Injectable({ providedIn: 'root' })
export class WorkflowRepository {
  getAll(): WorkflowSummary[] {
    return this.read().map(({ yaml, ...summary }) => summary);
  }

  getById(id: string): WorkflowDocument | undefined {
    return this.read().find((workflow) => workflow.id === id);
  }

  create(): WorkflowDocument {
    const id = crypto.randomUUID();
    return this.save({
      id,
      name: 'new-workflow',
      basisSet: 'B3_21G',
      packageRoot: 'C:\\MoleculesDb',
      processTypes: ['moleculeproperties'],
      yaml: STARTER_WORKFLOW_YAML,
    });
  }

  save(workflow: WorkflowDocument): WorkflowDocument {
    const updated = { ...workflow, ...this.extractSummary(workflow.yaml) };
    const workflows = this.read();
    const index = workflows.findIndex((item) => item.id === updated.id);

    if (index >= 0) {
      workflows[index] = updated;
    } else {
      workflows.push(updated);
    }

    localStorage.setItem(STORAGE_KEY, JSON.stringify(workflows));
    return updated;
  }

  delete(id: string): void {
    const workflows = this.read().filter((workflow) => workflow.id !== id);
    localStorage.setItem(STORAGE_KEY, JSON.stringify(workflows));
  }

  private read(): WorkflowDocument[] {
    const json = localStorage.getItem(STORAGE_KEY);
    if (!json) {
      localStorage.setItem(STORAGE_KEY, JSON.stringify(INITIAL_WORKFLOWS));
      return structuredClone(INITIAL_WORKFLOWS);
    }

    return JSON.parse(json) as WorkflowDocument[];
  }

  private extractSummary(yaml: string): Pick<WorkflowSummary, 'name' | 'basisSet' | 'packageRoot' | 'processTypes'> {
    const name = this.matchScalar(yaml, 'name') ?? 'Unnamed workflow';
    const basisSet = this.matchScalar(yaml, 'basisset') ?? '';
    const packageRoot = this.matchScalar(yaml, 'package_root') ?? '';
    const processTypes = [...yaml.matchAll(/^\s*-\s+type:\s*([^#\r\n]+)/gm)]
      .map((match) => match[1].trim())
      .filter((type) => type === 'moleculeproperties');

    return {
      name,
      basisSet,
      packageRoot: packageRoot.replace(/^['"]|['"]$/g, ''),
      processTypes: [...new Set(processTypes)],
    };
  }

  private matchScalar(yaml: string, key: string): string | undefined {
    const match = yaml.match(new RegExp(`^\\s*${key}:\\s*([^#\\r\\n]+)`, 'm'));
    return match?.[1].trim().replace(/^['"]|['"]$/g, '');
  }
}
