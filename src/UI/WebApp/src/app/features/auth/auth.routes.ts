import { Routes } from '@angular/router';

export const AUTH_ROUTES: Routes = [
  {
    path: 'login',
    loadComponent: () => import('./pages/login/login.page')
      .then(m => m.LoginPage)
  },
  {
    path: 'change-password',
    loadComponent: () => import('./pages/change-password/change-password.page')
      .then(m => m.ChangePasswordPage)
  },
  {
    path: 'forgot-password',
    loadComponent: () => import('./pages/forgot-password/forgot-password.page')
      .then(m => m.ForgotPasswordPage)
  },
  {
    path: 'reset-password',
    loadComponent: () => import('./pages/reset-password/reset-password.page')
      .then(m => m.ResetPasswordPage)
  }
];

