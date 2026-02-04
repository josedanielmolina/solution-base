import { Component, signal, ChangeDetectionStrategy, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '@core/services/auth.service';

@Component({
  selector: 'app-login-page',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './login.page.html',
  styleUrl: './login.page.css'
})
export class LoginPage {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);

  loading = signal<boolean>(false);
  errorMessage = signal<string>('');

  loginForm: FormGroup = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', Validators.required]
  });

  onSubmit(): void {
    if (this.loginForm.invalid) {
      return;
    }

    this.loading.set(true);
    this.errorMessage.set('');

    this.authService.login(this.loginForm.value).subscribe({
      next: (response) => {
        if (response.requiresPasswordChange) {
          this.router.navigate(['/auth/change-password']);
        } else {
          // Role-based redirect
          const redirectPath = this.getRedirectPath(response.user.roles);
          this.router.navigate([redirectPath]);
        }
      },
      error: (error) => {
        this.loading.set(false);
        const message = error.error?.message || 'Credenciales inválidas';
        this.errorMessage.set(message);
      }
    });
  }

  private getRedirectPath(roles: string[]): string {
    // PlatformAdmin goes to admin dashboard
    if (roles.includes('PlatformAdmin')) {
      return '/app/admin';
    }
    // Organizer and EventAdmin go to events
    return '/app/events';
  }
}

