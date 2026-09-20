import { Component, OnInit, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { WorkflowDocument } from './workflow.model';
import { WorkflowRepository } from './workflow.repository';

@Component({
  selector: 'app-workflow-editor',
  imports: [FormsModule],
  templateUrl: './workflow-editor.html',
  styleUrl: './workflow-editor.scss',
})
export class WorkflowEditor implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly repository = inject(WorkflowRepository);

  workflow?: WorkflowDocument;
  saved = false;
  loading = false;
  saving = false;
  errorMessage = '';

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) {
      void this.router.navigate(['/workflows']);
      return;
    }

    if (id === 'new') {
      this.workflow = this.repository.create();
      return;
    }

    this.loading = true;
    this.repository.getById(id).subscribe({
      next: (workflow) => {
        this.workflow = workflow;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        this.errorMessage = `Could not load workflow "${id}" from the WebAPI.`;
      },
    });
  }

  save(): void {
    if (!this.workflow || this.saving) return;

    this.saving = true;
    this.saved = false;
    this.errorMessage = '';

    this.repository.save(this.workflow).subscribe({
      next: (workflow) => {
        const wasNew = this.workflow?.id === 'new';
        this.workflow = workflow;
        this.saving = false;
        this.saved = true;

        if (wasNew) {
          void this.router.navigate(['/workflows', workflow.id], { replaceUrl: true });
        }
      },
      error: () => {
        this.saving = false;
        this.errorMessage = 'Could not save the workflow. Check the YAML and the WebAPI logs.';
      },
    });
  }

  cancel(): void {
    void this.router.navigate(['/workflows']);
  }
}
