import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, map } from 'rxjs';
import { ResearchDefinition, WorkflowDocument, WorkflowSummary } from './workflow.model';
import { STARTER_WORKFLOW_YAML } from './workflow-template';

const API_URL = '/api/ResearchDefinition';

@Injectable({ providedIn: 'root' })
export class WorkflowRepository {
  private readonly http = inject(HttpClient);

  getAll(): Observable<WorkflowSummary[]> {
    return this.http.get<ResearchDefinition[]>(API_URL).pipe(
      map((definitions) => definitions.map((definition) => this.toSummary(definition))),
    );
  }

  getById(id: string): Observable<WorkflowDocument> {
    return this.http
      .get(`${API_URL}/${encodeURIComponent(id)}/yaml`, { responseType: 'text' })
      .pipe(
        map((yaml) => ({
          id,
          yaml,
          ...this.extractSummary(yaml),
        })),
      );
  }

  create(): WorkflowDocument {
    return {
      id: 'new',
      yaml: STARTER_WORKFLOW_YAML,
      ...this.extractSummary(STARTER_WORKFLOW_YAML),
    };
  }

  save(workflow: WorkflowDocument): Observable<WorkflowDocument> {
    return this.http.post<void>(`${API_URL}/yaml`, { yaml: workflow.yaml }).pipe(
      map(() => {
        const summary = this.extractSummary(workflow.yaml);
        return {
          ...workflow,
          ...summary,
          id: summary.name,
        };
      }),
    );
  }

  delete(name: string): Observable<void> {
    return this.http.delete<void>(`${API_URL}/${encodeURIComponent(name)}`);
  }

  private toSummary(definition: ResearchDefinition): WorkflowSummary {
    return {
      id: definition.name,
      name: definition.name,
      basisSet: definition.basisset,
      packageRoot: definition.packageRoot,
      processTypes: definition.processes
        .map((process) => this.processTypeName(process.type))
        .filter((type) => type.length > 0),
    };
  }

  private processTypeName(type: number): string {
    switch (type) {
      case 1:
        return 'moleculeproperties';
      default:
        return type === 0 ? 'dummy' : `process-${type}`;
    }
  }

  private extractSummary(
    yaml: string,
  ): Pick<WorkflowSummary, 'name' | 'basisSet' | 'packageRoot' | 'processTypes'> {
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
