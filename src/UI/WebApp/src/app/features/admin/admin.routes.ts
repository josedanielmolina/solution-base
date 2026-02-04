import { Routes } from '@angular/router';
import { permissionGuard } from '@core/guards/permission.guard';

export const ADMIN_ROUTES: Routes = [
    {
        path: 'roles',
        loadComponent: () => import('./pages/role-list/role-list.page')
            .then(m => m.RoleListPage),
        canActivate: [permissionGuard],
        data: { permissions: ['roles.view'] }
    },
    {
        path: 'roles/:id/permissions',
        loadComponent: () => import('./pages/role-permissions/role-permissions.page')
            .then(m => m.RolePermissionsPage),
        canActivate: [permissionGuard],
        data: { permissions: ['roles.configure'] }
    },
    {
        path: 'countries',
        loadChildren: () => import('./countries/countries.routes')
            .then(m => m.COUNTRIES_ROUTES)
    },
    {
        path: 'cities',
        loadChildren: () => import('./cities/cities.routes')
            .then(m => m.CITIES_ROUTES)
    },
    {
        path: 'categories',
        loadChildren: () => import('./categories/categories.routes')
            .then(m => m.CATEGORIES_ROUTES)
    },
    {
        path: 'establishments',
        loadChildren: () => import('./establishments/establishments.routes')
            .then(m => m.ESTABLISHMENTS_ROUTES),
        canActivate: [permissionGuard],
        data: { permissions: ['establishments.view'] }
    }
];

