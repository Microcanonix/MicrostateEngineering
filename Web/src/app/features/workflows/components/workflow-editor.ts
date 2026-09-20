import { ChangeDetectorRef, Component, OnInit, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import {
  PROCESS_TYPE_OPTIONS,
  STEP_TYPE_OPTIONS,
  ProcessType,
  ResearchDefinition,
  ResearchDefinitionProcess,
  StepType,
  createResearchDefinition,
} from '../models/research-definition.model';
import { ResearchDefinitionService, describeHttpError } from '../services/research-definition.service';

@Component({
  selector: 'app-workflow-editor',
  imports: [FormsModule],
  templateUrl: './workflow-editor.html',
  styleUrl: './workflow-editor.scss',
})
export class WorkflowEditor implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly researchDefinitions = inject(ResearchDefinitionService);
  private readonly changeDetector = inject(ChangeDetectorRef);

  readonly processTypeOptions = PROCESS_TYPE_OPTIONS;
  readonly stepTypeOptions = STEP_TYPE_OPTIONS;

  definition?: ResearchDefinition;
  isNew = false;
  loading = false;
  saving = false;
  saved = false;
  errorMessage = '';

  ngOnInit(): void {
    const name = this.route.snapshot.paramMap.get('name');
    if (!name) {
      void this.router.navigate(['/workflows']);
      return;
    }

    if (name === 'new') {
      this.isNew = true;
      this.definition = createResearchDefinition();
      return;
    }

    this.loading = true;
    this.researchDefinitions.getByName(name).subscribe({
      next: (definition) => {
        this.definition = definition;
        this.loading = false;
        this.changeDetector.markForCheck();
      },
      error: (error) => {
        console.error('Load research definition failed', error);
        this.loading = false;
        this.errorMessage = `Could not load research definition "${name}". ${describeHttpError(error)}`;
        this.changeDetector.markForCheck();
      },
    });
  }

  save(): void {
    if (!this.definition || this.saving) {
      return;
    }

    this.definition.name = this.definition.name.trim();
    if (!this.definition.name) {
      this.errorMessage = 'Name is required.';
      return;
    }

    this.saving = true;
    this.saved = false;
    this.errorMessage = '';

    const wasNew = this.isNew;
    this.researchDefinitions.save(this.definition).subscribe({
      next: () => {
        this.saving = false;
        this.saved = true;
        this.isNew = false;

        if (wasNew) {
          void this.router.navigate(['/workflows', this.definition!.name], { replaceUrl: true });
        }

        this.changeDetector.markForCheck();
      },
      error: (error) => {
        console.error('Save research definition failed', error);
        this.saving = false;
        this.errorMessage = `Could not save the research definition. ${describeHttpError(error)}`;
        this.changeDetector.markForCheck();
      },
    });
  }

  cancel(): void {
    void this.router.navigate(['/workflows']);
  }

  addMolecule(): void {
    this.definition?.molecules.push({ name: '', charge: 0 });
    this.saved = false;
  }

  removeMolecule(index: number): void {
    this.definition?.molecules.splice(index, 1);
    this.saved = false;
  }

  addProcess(): void {
    this.definition?.processes.push({
      type: ProcessType.MoleculeProperties,
      steps: [],
      dependencies: [],
    });
    this.saved = false;
  }

  removeProcess(index: number): void {
    this.definition?.processes.splice(index, 1);
    this.saved = false;
  }

  addStep(process: ResearchDefinitionProcess): void {
    const nextId =
      process.steps.length === 0
        ? 0
        : Math.max(...process.steps.map((step) => step.id)) + 1;

    process.steps.push({
      id: nextId,
      type: StepType.Dummy,
      canExecute: true,
    });
    this.saved = false;
  }

  removeStep(process: ResearchDefinitionProcess, index: number): void {
    process.steps.splice(index, 1);
    this.saved = false;
  }

  addDependency(process: ResearchDefinitionProcess): void {
    process.dependencies.push({
      dependency: StepType.Dummy,
      dependant: StepType.Dummy,
    });
    this.saved = false;
  }

  removeDependency(process: ResearchDefinitionProcess, index: number): void {
    process.dependencies.splice(index, 1);
    this.saved = false;
  }
}
