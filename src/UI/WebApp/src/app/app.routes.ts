import { Routes } from '@angular/router';
import { authGuard } from '@core/guards/auth.guard';
import { MainLayoutComponent } from '@layouts/main-layout/main-layout.component';
import { ComponentKitLayoutComponent } from '@layouts/component-kit-layout/component-kit-layout.component';

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
    path: 'component-kit',
    component: ComponentKitLayoutComponent
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
