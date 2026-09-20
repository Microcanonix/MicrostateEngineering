import { Routes } from '@angular/router';
import { WorkflowEditor } from './features/workflows/components/workflow-editor';
import { WorkflowList } from './features/workflows/components/workflow-list';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'workflows' },
  { path: 'workflows', component: WorkflowList },
  { path: 'workflows/:name', component: WorkflowEditor },
  { path: '**', redirectTo: 'workflows' },
];
