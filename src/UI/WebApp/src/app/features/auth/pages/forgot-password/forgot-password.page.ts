import { Component, signal, ChangeDetectionStrategy, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '@core/services/auth.service';

@Component({
    selector: 'app-forgot-password-page',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule, RouterLink],
    changeDetection: ChangeDetectionStrategy.OnPush,
    templateUrl: './forgot-password.page.html',
    styleUrl: './forgot-password.page.css'
})
export class ForgotPasswordPage {
    private fb = inject(FormBuilder);
    private authService = inject(AuthService);
    private router = inject(Router);

    loading = signal<boolean>(false);
    errorMessage = signal<string>('');
    emailSent = signal<boolean>(false);

    emailForm: FormGroup = this.fb.group({
        email: ['', [Validators.required, Validators.email]]
    });

    onSubmit(): void {
        if (this.emailForm.invalid) {
            return;
        }

        this.loading.set(true);
        this.errorMessage.set('');

        this.authService.requestPasswordReset(this.emailForm.value).subscribe({
            next: () => {
                this.loading.set(false);
                this.emailSent.set(true);
            },
            error: () => {
                // Always show success to prevent email enumeration
                this.loading.set(false);
                this.emailSent.set(true);
            }
        });
    }

    goToReset(): void {
        const email = this.emailForm.get('email')?.value;
        this.router.navigate(['/auth/reset-password'], { queryParams: { email } });
    }
}
