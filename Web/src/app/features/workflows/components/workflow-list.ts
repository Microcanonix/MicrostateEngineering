import { ChangeDetectorRef, Component, OnInit, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import {
  ProcessType,
  ResearchDefinition,
  processTypeLabel,
} from '../models/research-definition.model';
import { ResearchDefinitionService, describeHttpError } from '../services/research-definition.service';

@Component({
  selector: 'app-workflow-list',
  imports: [RouterLink],
  templateUrl: './workflow-list.html',
})
export class WorkflowList implements OnInit {
  private readonly researchDefinitions = inject(ResearchDefinitionService);
  private readonly router = inject(Router);
  private readonly changeDetector = inject(ChangeDetectorRef);

  definitions: ResearchDefinition[] = [];
  loading = false;
  errorMessage = '';

  ngOnInit(): void {
    this.load();
  }

  addWorkflow(): void {
    void this.router.navigate(['/workflows', 'new']);
  }

  deleteWorkflow(definition: ResearchDefinition): void {
    const confirmed = window.confirm(`Delete research definition "${definition.name}"?`);
    if (!confirmed) {
      return;
    }

    this.errorMessage = '';
    this.researchDefinitions.delete(definition.name).subscribe({
      next: () => this.load(),
      error: (error) => {
        console.error('Delete research definition failed', error);
        this.errorMessage = `Could not delete research definition "${definition.name}". ${describeHttpError(error)}`;
        this.changeDetector.markForCheck();
      },
    });
  }

  processLabel(type: ProcessType): string {
    return processTypeLabel(type);
  }

  private load(): void {
    this.loading = true;
    this.errorMessage = '';

    this.researchDefinitions.getAll().subscribe({
      next: (definitions) => {
        this.definitions = definitions;
        this.loading = false;
        this.changeDetector.markForCheck();
      },
      error: (error) => {
        console.error('Load research definitions failed', error);
        this.definitions = [];
        this.loading = false;
        this.errorMessage = `Could not load research definitions from the WebAPI. ${describeHttpError(error)}`;
        this.changeDetector.markForCheck();
      },
    });
  }
}
