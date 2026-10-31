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
  moleculesExpanded = true;
  moleculeFilter = '';
  filteredMolecules: MoleculeScientificReport[] = [];

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
        this.applyMoleculeFilter();
        if (this.filteredMolecules.length) {
          this.selectMolecule(this.filteredMolecules[0]);
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

  toggleMolecules(): void {
    this.moleculesExpanded = !this.moleculesExpanded;
    this.changeDetector.markForCheck();
  }

  setMoleculeFilter(filter: string): void {
    this.moleculeFilter = filter;
    this.applyMoleculeFilter();
    this.changeDetector.markForCheck();
  }

  private applyMoleculeFilter(): void {
    const molecules = this.report?.moleculeReports ?? [];
    const needle = this.moleculeFilter.trim().toLowerCase();
    this.filteredMolecules = needle
      ? molecules.filter((molecule) => molecule.moleculeName.toLowerCase().includes(needle))
      : molecules;
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
