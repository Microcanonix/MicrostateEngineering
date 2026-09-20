import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, map } from 'rxjs';
import { ResearchDefinition } from './research-definition.model';

const API_URL = '/api/ResearchDefinition';

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
