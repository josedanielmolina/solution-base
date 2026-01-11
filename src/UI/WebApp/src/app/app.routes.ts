import { Routes } from '@angular/router';
import { authGuard } from '@core/guards/auth.guard';
import { MainLayoutComponent } from '@layouts/main-layout/main-layout.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: '/app',
    pathMatch: 'full'
  },
  {
    path: 'login',
    loadComponent: () => import('./features/auth/pages/login/login.page')
      .then(m => m.LoginPage)
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
        loadChildren: () => import('./features/users/users.routes')
          .then(m => m.USERS_ROUTES)
      }
    ]
  },
  {
    path: '**',
    redirectTo: '/app'
  }
];
