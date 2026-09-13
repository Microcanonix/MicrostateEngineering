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

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) {
      void this.router.navigate(['/workflows']);
      return;
    }

    this.workflow = this.repository.getById(id);
    if (!this.workflow) {
      void this.router.navigate(['/workflows']);
    }
  }

  save(): void {
    if (!this.workflow) return;
    this.workflow = this.repository.save(this.workflow);
    this.saved = true;
  }

  cancel(): void {
    void this.router.navigate(['/workflows']);
  }
}
