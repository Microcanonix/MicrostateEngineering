import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { ResearchDefinitionScientificReport } from '../models/scientific-report.model';

const API_URL = 'https://localhost:7094/ScientificReport';

@Injectable({ providedIn: 'root' })
export class ScientificReportService {
  private readonly http = inject(HttpClient);

  getResearchDefinitionReport(researchDefinitionName: string): Observable<ResearchDefinitionScientificReport | null> {
    return this.http.get<ResearchDefinitionScientificReport | null>(
      `${API_URL}/${encodeURIComponent(researchDefinitionName)}`,
    );
  }
}
