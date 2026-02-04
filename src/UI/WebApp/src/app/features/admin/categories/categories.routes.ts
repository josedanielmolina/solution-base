import { Routes } from '@angular/router';
import { permissionGuard } from '@core/guards/permission.guard';

export const CATEGORIES_ROUTES: Routes = [
    {
        path: '',
        loadComponent: () => import('./pages/category-list/category-list.page')
            .then(m => m.CategoryListPage),
        canActivate: [permissionGuard],
        data: { permissions: ['categories.view'] }
    },
    {
        path: 'new',
        loadComponent: () => import('./pages/category-form/category-form.page')
            .then(m => m.CategoryFormPage),
        canActivate: [permissionGuard],
        data: { permissions: ['categories.create'] }
    },
    {
        path: ':id/edit',
        loadComponent: () => import('./pages/category-form/category-form.page')
            .then(m => m.CategoryFormPage),
        canActivate: [permissionGuard],
        data: { permissions: ['categories.edit'] }
    }
];
