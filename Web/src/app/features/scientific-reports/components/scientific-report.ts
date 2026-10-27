import { ChangeDetectorRef, Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { MoleculeScientificReport, ResearchDefinitionScientificReport } from '../models/scientific-report.model';
import { ScientificReportService } from '../services/scientific-report.service';
import { describeHttpError } from '../../workflows/services/research-definition.service';
import { MoleculeReportExplorer } from '../../molecule-report-explorer/components/molecule-report-explorer';

@Component({
  selector: 'app-scientific-report',
  imports: [RouterLink, MoleculeReportExplorer],
  templateUrl: './scientific-report.html',
  styleUrls: ['./scientific-report.scss'],
})
export class ScientificReport implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly changeDetector = inject(ChangeDetectorRef);
  private readonly scientificReports = inject(ScientificReportService);

  name = '';
  report?: ResearchDefinitionScientificReport;
  selectedMolecule?: MoleculeScientificReport;
  selectedReportJson = '';
  loading = false;
  errorMessage = '';

  ngOnInit(): void {
    this.name = this.route.snapshot.paramMap.get('name') ?? '';
    if (!this.name) {
      void this.router.navigate(['/workflows']);
      return;
    }

    this.loading = true;
    this.errorMessage = '';

    this.scientificReports.getResearchDefinitionReport(this.name).subscribe({
      next: (report) => {
        this.report = report ?? undefined;
        this.loading = false;
        if (this.report?.moleculeReports.length) {
          this.selectMolecule(this.report.moleculeReports[0]);
        }
        this.changeDetector.markForCheck();
      },
      error: (error) => {
        console.error('Load scientific report failed', error);
        this.loading = false;
        this.report = undefined;
        this.selectedMolecule = undefined;
        this.selectedReportJson = '';
        this.errorMessage = `Could not load the scientific report for research definition \"${this.name}\". ${describeHttpError(error)}`;
        this.changeDetector.markForCheck();
      },
    });
  }

  selectMolecule(molecule: MoleculeScientificReport): void {
    this.selectedMolecule = molecule;
    try {
      this.selectedReportJson = molecule.report
        ? JSON.stringify(molecule.report, null, 2)
        : '';
    } catch {
      this.selectedReportJson = String(molecule.report);
    }
  }

  downloadSelectedReportJson(): void {
    const molecule = this.selectedMolecule;
    if (!molecule?.report) {
      return;
    }

    try {
      const json = JSON.stringify(molecule.report, null, 2);
      const blob = new Blob([json], { type: 'application/json' });
      const url = URL.createObjectURL(blob);
      const anchor = document.createElement('a');
      anchor.href = url;
      anchor.download = `${this.name}-${molecule.moleculeName}-report.json`;
      anchor.click();
      URL.revokeObjectURL(url);
    } catch (error) {
      console.error('Download report JSON failed', error);
      this.errorMessage = `Could not download the report JSON for "${molecule.moleculeName}".`;
      this.changeDetector.markForCheck();
    }
  }
}
