import { Routes } from '@angular/router';

export const ESTABLISHMENTS_ROUTES: Routes = [
    {
        path: '',
        loadComponent: () => import('./pages/establishment-list/establishment-list.page')
            .then(m => m.EstablishmentListPage)
    },
    {
        path: 'new',
        loadComponent: () => import('./pages/establishment-form/establishment-form.page')
            .then(m => m.EstablishmentFormPage)
    },
    {
        path: ':id',
        loadComponent: () => import('./pages/establishment-detail/establishment-detail.page')
            .then(m => m.EstablishmentDetailPage)
    },
    {
        path: ':id/edit',
        loadComponent: () => import('./pages/establishment-form/establishment-form.page')
            .then(m => m.EstablishmentFormPage)
    }
];
