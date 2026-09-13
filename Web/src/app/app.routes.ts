import { Routes } from '@angular/router';
import { WorkflowEditor } from './workflows/workflow-editor';
import { WorkflowList } from './workflows/workflow-list';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'workflows' },
  { path: 'workflows', component: WorkflowList },
  { path: 'workflows/:id', component: WorkflowEditor },
  { path: '**', redirectTo: 'workflows' },
];
