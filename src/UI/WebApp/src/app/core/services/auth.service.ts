import { Injectable, signal, computed, inject } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { environment } from '@environments/environment';
import { StorageUtils } from '@core/utils/common.utils';
import {
  LoginCredentials,
  AuthUser,
  LoginResponse,
  ChangePasswordRequest,
  RequestPasswordResetRequest,
  ResetPasswordRequest
} from '@core/models/auth.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly TOKEN_KEY = 'auth_token';
  private readonly USER_KEY = 'auth_user';
  private readonly PERMISSIONS_KEY = 'user_permissions';

  private http = inject(HttpClient);
  private router = inject(Router);

  // Signals
  token = signal<string | null>(this.getToken());
  currentUser = signal<AuthUser | null>(this.getUser());
  requiresPasswordChange = signal<boolean>(false);
  isAuthenticated = computed(() => !!this.token());

  // Permission check
  hasPermission(permission: string): boolean {
    const user = this.currentUser();
    if (!user) return false;
    return user.permissions.includes(permission);
  }

  hasAnyPermission(permissions: string[]): boolean {
    return permissions.some(p => this.hasPermission(p));
  }

  hasAllPermissions(permissions: string[]): boolean {
    return permissions.every(p => this.hasPermission(p));
  }

  hasRole(role: string): boolean {
    const user = this.currentUser();
    if (!user) return false;
    return user.roles.includes(role);
  }

  login(credentials: LoginCredentials): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${environment.apiUrl}/api/auth/login`, credentials)
      .pipe(
        tap(response => {
          this.setToken(response.token);
          this.setUser(response.user);
          this.token.set(response.token);
          this.currentUser.set(response.user);
          this.requiresPasswordChange.set(response.requiresPasswordChange);
        })
      );
  }

  changePassword(request: ChangePasswordRequest): Observable<void> {
    return this.http.post<void>(`${environment.apiUrl}/api/auth/change-password`, request)
      .pipe(
        tap(() => {
          this.requiresPasswordChange.set(false);
        })
      );
  }

  requestPasswordReset(request: RequestPasswordResetRequest): Observable<void> {
    return this.http.post<void>(`${environment.apiUrl}/api/auth/request-password-reset`, request);
  }

  resetPassword(request: ResetPasswordRequest): Observable<void> {
    return this.http.post<void>(`${environment.apiUrl}/api/auth/reset-password`, request);
  }

  getCurrentUser(): Observable<AuthUser> {
    return this.http.get<AuthUser>(`${environment.apiUrl}/api/auth/me`)
      .pipe(
        tap(user => {
          this.setUser(user);
          this.currentUser.set(user);
        })
      );
  }

  logout(): void {
    this.clearAuth();
    this.router.navigate(['/login']);
  }

  getToken(): string | null {
    return StorageUtils.get<string>(this.TOKEN_KEY);
  }

  getUser(): AuthUser | null {
    return StorageUtils.get<AuthUser>(this.USER_KEY);
  }

  isLoggedIn(): boolean {
    const token = this.getToken();
    if (!token) {
      return false;
    }

    // Check if token is expired
    try {
      const payload = this.decodeToken(token);
      const expirationDate = new Date(payload.exp * 1000);
      return expirationDate > new Date();
    } catch {
      return false;
    }
  }

  private setToken(token: string): void {
    StorageUtils.set(this.TOKEN_KEY, token);
  }

  private setUser(user: AuthUser): void {
    StorageUtils.set(this.USER_KEY, user);
  }

  private clearAuth(): void {
    StorageUtils.remove(this.TOKEN_KEY);
    StorageUtils.remove(this.USER_KEY);
    this.token.set(null);
    this.currentUser.set(null);
    this.requiresPasswordChange.set(false);
  }

  private decodeToken(token: string): any {
    const parts = token.split('.');
    if (parts.length !== 3) {
      throw new Error('Invalid token');
    }

    const payload = parts[1];
    const decoded = atob(payload);
    return JSON.parse(decoded);
  }

  refreshAuthState(): void {
    const token = this.getToken();
    const user = this.getUser();

    if (token && user && this.isLoggedIn()) {
      this.token.set(token);
      this.currentUser.set(user);
    } else {
      this.clearAuth();
    }
  }
}

