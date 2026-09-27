import { Routes } from '@angular/router';
import { WorkflowEditor } from './features/workflows/components/workflow-editor';
import { WorkflowList } from './features/workflows/components/workflow-list';
import { WorkflowRun } from './features/workflows/components/workflow-run';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'workflows' },
  { path: 'workflows', component: WorkflowList },
  { path: 'workflows/:name/run', component: WorkflowRun },
  { path: 'workflows/:name', component: WorkflowEditor },
  { path: '**', redirectTo: 'workflows' },
];
