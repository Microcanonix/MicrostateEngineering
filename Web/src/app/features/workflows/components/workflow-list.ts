import { Component, OnInit, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { WorkflowSummary } from './workflow.model';
import { WorkflowRepository } from './workflow.repository';

@Component({
  selector: 'app-workflow-list',
  imports: [RouterLink],
  templateUrl: './workflow-list.html',
})
export class WorkflowList implements OnInit {
  private readonly repository = inject(WorkflowRepository);
  private readonly router = inject(Router);

  workflows: WorkflowSummary[] = [];
  loading = false;
  errorMessage = '';

  ngOnInit(): void {
    this.loadWorkflows();
  }

  addWorkflow(): void {
    void this.router.navigate(['/workflows', 'new']);
  }

  deleteWorkflow(workflow: WorkflowSummary): void {
    const confirmed = window.confirm(`Delete workflow "${workflow.name}"?`);
    if (!confirmed) {
      return;
    }

    this.errorMessage = '';
    this.repository.delete(workflow.name).subscribe({
      next: () => this.loadWorkflows(),
      error: () => {
        this.errorMessage = `Could not delete workflow "${workflow.name}".`;
      },
    });
  }

  private loadWorkflows(): void {
    this.loading = true;
    this.errorMessage = '';

    this.repository.getAll().subscribe({
      next: (workflows) => {
        this.workflows = workflows;
        this.loading = false;
      },
      error: () => {
        this.workflows = [];
        this.loading = false;
        this.errorMessage = 'Could not load workflows from the WebAPI.';
      },
    });
  }
}
