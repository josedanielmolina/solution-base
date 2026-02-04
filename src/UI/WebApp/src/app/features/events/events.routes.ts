import { Routes } from '@angular/router';

export const eventsRoutes: Routes = [
    {
        path: '',
        loadComponent: () => import('./pages/event-list/event-list.page').then(m => m.EventListPage)
    },
    {
        path: 'new',
        loadComponent: () => import('./pages/event-form/event-form.page').then(m => m.EventFormPage)
    },
    {
        path: ':id/edit',
        loadComponent: () => import('./pages/event-form/event-form.page').then(m => m.EventFormPage)
    },
    // Event Admin routes will be nested under :id
    {
        path: ':id',
        loadChildren: () => import('./event-admin/event-admin.routes').then(m => m.eventAdminRoutes)
    }
];
