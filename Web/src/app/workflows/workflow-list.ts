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

  ngOnInit(): void {
    this.loadWorkflows();
  }

  addWorkflow(): void {
    const workflow = this.repository.create();
    void this.router.navigate(['/workflows', workflow.id]);
  }

  deleteWorkflow(workflow: WorkflowSummary): void {
    const confirmed = window.confirm(`Delete workflow "${workflow.name}"?`);
    if (!confirmed) {
      return;
    }

    this.repository.delete(workflow.id);
    this.loadWorkflows();
  }

  private loadWorkflows(): void {
    this.workflows = this.repository.getAll();
  }
}
