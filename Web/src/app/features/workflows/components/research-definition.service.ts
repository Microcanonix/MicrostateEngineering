import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, map } from 'rxjs';
import { ResearchDefinition } from './research-definition.model';

const API_URL = 'https://localhost:7094/ResearchDefinition';

@Injectable({ providedIn: 'root' })
export class ResearchDefinitionService {
  private readonly http = inject(HttpClient);

  getAll(): Observable<ResearchDefinition[]> {
    return this.http.get<ResearchDefinition[]>(API_URL);
  }

  getByName(name: string): Observable<ResearchDefinition> {
    return this.getAll().pipe(
      map((definitions) => {
        const definition = definitions.find(
          (item) => item.name.toLowerCase() === name.toLowerCase(),
        );

        if (!definition) {
          throw new Error(`Research definition "${name}" was not found.`);
        }

        return structuredClone(definition);
      }),
    );
  }

  save(definition: ResearchDefinition): Observable<void> {
    return this.http.post<void>(API_URL, definition);
  }

  delete(name: string): Observable<void> {
    return this.http.delete<void>(`${API_URL}/${encodeURIComponent(name)}`);
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
