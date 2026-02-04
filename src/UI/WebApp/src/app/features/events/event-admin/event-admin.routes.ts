import { Routes } from '@angular/router';

export const eventAdminRoutes: Routes = [
    {
        path: '',
        loadComponent: () =>
            import('./layouts/event-layout.component').then(m => m.EventLayoutComponent),
        children: [
            {
                path: '',
                loadComponent: () =>
                    import('./pages/overview/overview.page').then(m => m.EventOverviewPage)
            },
            {
                path: 'establishments',
                loadComponent: () =>
                    import('./pages/establishments/establishments.page').then(m => m.EventEstablishmentsPage)
            },
            {
                path: 'admins',
                loadComponent: () =>
                    import('./pages/admins/admins.page').then(m => m.EventAdminsPage)
            },
            {
                path: 'tournaments',
                loadComponent: () =>
                    import('./pages/tournaments/tournaments.page').then(m => m.EventTournamentsPage)
            },
            {
                path: 'settings',
                loadComponent: () =>
                    import('./pages/settings/settings.page').then(m => m.EventSettingsPage)
            }
        ]
    }
];
