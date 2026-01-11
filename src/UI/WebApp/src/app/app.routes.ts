import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { MainLayoutComponent } from './layouts/main-layout/main-layout.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: '/app',
    pathMatch: 'full'
  },
  {
    path: 'login',
    loadComponent: () => import('./features/auth/components/login.component').then(m => m.LoginComponent)
  },
  {
    path: 'app',
    component: MainLayoutComponent,
    canActivate: [authGuard],
    children: [
      {
        path: '',
        redirectTo: 'users',
        pathMatch: 'full'
      },
      {
        path: 'users',
        children: [
          {
            path: '',
            loadComponent: () => import('./features/users/components/user-list/user-list.component').then(m => m.UserListComponent)
          },
          {
            path: 'create',
            loadComponent: () => import('./features/users/components/user-form/user-form.component').then(m => m.UserFormComponent)
          },
          {
            path: ':id',
            loadComponent: () => import('./features/users/components/user-detail/user-detail.component').then(m => m.UserDetailComponent)
          },
          {
            path: ':id/edit',
            loadComponent: () => import('./features/users/components/user-form/user-form.component').then(m => m.UserFormComponent)
          }
        ]
      }
    ]
  },
  {
    path: '**',
    redirectTo: '/app'
  }
];
