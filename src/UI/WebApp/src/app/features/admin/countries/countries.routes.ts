import { Routes } from '@angular/router';
import { permissionGuard } from '@core/guards/permission.guard';

export const COUNTRIES_ROUTES: Routes = [
    {
        path: '',
        loadComponent: () => import('./pages/country-list/country-list.page')
            .then(m => m.CountryListPage),
        canActivate: [permissionGuard],
        data: { permissions: ['countries.view'] }
    },
    {
        path: 'new',
        loadComponent: () => import('./pages/country-form/country-form.page')
            .then(m => m.CountryFormPage),
        canActivate: [permissionGuard],
        data: { permissions: ['countries.create'] }
    },
    {
        path: ':id/edit',
        loadComponent: () => import('./pages/country-form/country-form.page')
            .then(m => m.CountryFormPage),
        canActivate: [permissionGuard],
        data: { permissions: ['countries.edit'] }
    }
];
