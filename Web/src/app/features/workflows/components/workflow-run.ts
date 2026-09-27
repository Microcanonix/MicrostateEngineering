import { ChangeDetectorRef, Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ResearchDefinition } from '../models/research-definition.model';
import { WorkflowRunReport } from '../models/workflow-run.model';
import { ResearchDefinitionService, describeHttpError } from '../services/research-definition.service';
import { WorkflowRunService } from '../services/workflow-run.service';

@Component({
  selector: 'app-workflow-run',
  imports: [RouterLink],
  templateUrl: './workflow-run.html',
})
export class WorkflowRun implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly changeDetector = inject(ChangeDetectorRef);
  private readonly researchDefinitions = inject(ResearchDefinitionService);
  private readonly workflowRuns = inject(WorkflowRunService);

  name = '';
  definition?: ResearchDefinition;
  loading = false;
  running = false;
  report?: WorkflowRunReport;
  errorMessage = '';

  ngOnInit(): void {
    this.name = this.route.snapshot.paramMap.get('name') ?? '';
    if (!this.name) {
      void this.router.navigate(['/workflows']);
      return;
    }

    this.loading = true;
    this.researchDefinitions.getByName(this.name).subscribe({
      next: (definition) => {
        this.definition = definition;
        this.loading = false;
        this.changeDetector.markForCheck();
      },
      error: (error) => {
        console.error('Load research definition failed', error);
        this.loading = false;
        this.errorMessage = `Could not load research definition "${this.name}". ${describeHttpError(error)}`;
        this.changeDetector.markForCheck();
      },
    });
  }

  runWorkflow(): void {
    if (this.running || !this.definition) {
      return;
    }

    this.running = true;
    this.errorMessage = '';
    this.report = undefined;

    this.workflowRuns.run(this.definition.name).subscribe({
      next: (report) => {
        this.running = false;
        this.report = report;
        this.changeDetector.markForCheck();
      },
      error: (error) => {
        console.error('Run workflow failed', error);
        this.running = false;
        this.errorMessage = `Could not run workflow "${this.definition!.name}". ${describeHttpError(error)}`;
        this.changeDetector.markForCheck();
      },
    });
  }

  get succeededCount(): number {
    return this.report?.moleculeResult.filter((item) => item.succeeded).length ?? 0;
  }

  get failedCount(): number {
    return this.report?.moleculeResult.filter((item) => !item.succeeded).length ?? 0;
  }
}
