import { Routes } from '@angular/router';
import { permissionGuard } from '@core/guards/permission.guard';

export const CITIES_ROUTES: Routes = [
    {
        path: '',
        loadComponent: () => import('./pages/city-list/city-list.page')
            .then(m => m.CityListPage),
        canActivate: [permissionGuard],
        data: { permissions: ['cities.view'] }
    },
    {
        path: 'new',
        loadComponent: () => import('./pages/city-form/city-form.page')
            .then(m => m.CityFormPage),
        canActivate: [permissionGuard],
        data: { permissions: ['cities.create'] }
    },
    {
        path: ':id/edit',
        loadComponent: () => import('./pages/city-form/city-form.page')
            .then(m => m.CityFormPage),
        canActivate: [permissionGuard],
        data: { permissions: ['cities.edit'] }
    }
];
