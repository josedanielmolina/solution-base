import { Routes } from '@angular/router';

export const USERS_ROUTES: Routes = [
  {
    path: 'profile',
    loadComponent: () => import('./pages/profile/profile.page')
      .then(m => m.ProfilePage)
  },
  {
    path: '',
    loadComponent: () => import('./pages/user-list/user-list.page')
      .then(m => m.UserListPage)
  },
  {
    path: 'create',
    loadComponent: () => import('./pages/user-form/user-form.page')
      .then(m => m.UserFormPage)
  },
  {
    path: ':id',
    loadComponent: () => import('./pages/user-detail/user-detail.page')
      .then(m => m.UserDetailPage)
  },
  {
    path: ':id/edit',
    loadComponent: () => import('./pages/user-form/user-form.page')
      .then(m => m.UserFormPage)
  }
];

