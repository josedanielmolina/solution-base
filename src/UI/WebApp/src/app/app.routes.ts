import { Routes } from '@angular/router';
import { authGuard } from '@core/guards/auth.guard';
import { passwordChangeGuard } from '@core/guards/password-change.guard';
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
    path: 'auth',
    loadChildren: () => import('./features/auth/auth.routes')
      .then(m => m.AUTH_ROUTES)
  },
  {
    path: 'app',
    component: MainLayoutComponent,
    canActivate: [authGuard, passwordChangeGuard],
    children: [
      {
        path: '',
        redirectTo: 'events',
        pathMatch: 'full'
      },
      {
        path: 'events',
        loadChildren: () => import('./features/events/events.routes')
          .then(m => m.eventsRoutes)
      },
      {
        path: 'users',
        loadChildren: () => import('./features/users/users.routes')
          .then(m => m.USERS_ROUTES)
      },
      {
        path: 'admin',
        loadChildren: () => import('./features/admin/admin.routes')
          .then(m => m.ADMIN_ROUTES)
      }
    ]
  },
  {
    path: 'unauthorized',
    loadComponent: () => import('./features/auth/pages/login/login.page')
      .then(m => m.LoginPage) // TODO: Create unauthorized page
  },
  {
    path: '**',
    redirectTo: '/app'
  }
];

