import { inject } from '@angular/core';
import { Router, CanActivateFn } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const passwordChangeGuard: CanActivateFn = () => {
    const authService = inject(AuthService);
    const router = inject(Router);

    if (!authService.isLoggedIn()) {
        router.navigate(['/login']);
        return false;
    }

    // If user requires password change, redirect to change password page
    if (authService.requiresPasswordChange()) {
        router.navigate(['/auth/change-password']);
        return false;
    }

    return true;
};
