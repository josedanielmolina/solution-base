import { inject } from '@angular/core';
import { Router, CanActivateFn, ActivatedRouteSnapshot } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const permissionGuard: CanActivateFn = (route: ActivatedRouteSnapshot) => {
    const authService = inject(AuthService);
    const router = inject(Router);

    // First check authentication
    if (!authService.isLoggedIn()) {
        router.navigate(['/login']);
        return false;
    }

    // Get required permissions from route data
    const requiredPermissions = route.data['permissions'] as string[] | undefined;
    const requireAll = route.data['requireAllPermissions'] as boolean | undefined;

    if (!requiredPermissions || requiredPermissions.length === 0) {
        return true;
    }

    const hasAccess = requireAll
        ? authService.hasAllPermissions(requiredPermissions)
        : authService.hasAnyPermission(requiredPermissions);

    if (!hasAccess) {
        // Redirect to unauthorized page or home
        router.navigate(['/unauthorized']);
        return false;
    }

    return true;
};
