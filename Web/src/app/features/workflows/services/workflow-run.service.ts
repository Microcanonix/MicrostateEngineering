import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { WorkflowRunReport } from '../models/workflow-run.model';

const API_URL = 'https://localhost:7094/Workflow';

@Injectable({ providedIn: 'root' })
export class WorkflowRunService {
  private readonly http = inject(HttpClient);

  run(researchDefinitionName: string): Observable<WorkflowRunReport> {
    return this.http.post<WorkflowRunReport>(API_URL, null, {
      params: { researchDefintionName: researchDefinitionName },
    });
  }
}

export function describeHttpError(error: unknown): string {
  if (error instanceof HttpErrorResponse) {
    const location = error.url ? ` ${error.url}` : '';
    const status = error.status === 0
      ? 'Network error'
      : `HTTP ${error.status} ${error.statusText}`;

    let details = '';

    if (typeof error.error === 'string' && error.error.trim()) {
      details = error.error.trim();
    } else if (error.error && typeof error.error === 'object') {
      try {
        details = JSON.stringify(error.error);
      } catch {
        details = String(error.error);
      }
    } else if (error.message) {
      details = error.message;
    }

    return details
      ? `${status}${location}: ${details}`
      : `${status}${location}`;
  }

  if (error instanceof Error) {
    return error.message;
  }

  return String(error);
}
