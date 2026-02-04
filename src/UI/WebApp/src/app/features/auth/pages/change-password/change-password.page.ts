import { Component, signal, ChangeDetectionStrategy, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '@core/services/auth.service';

@Component({
    selector: 'app-change-password-page',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule],
    changeDetection: ChangeDetectionStrategy.OnPush,
    templateUrl: './change-password.page.html',
    styleUrl: './change-password.page.css'
})
export class ChangePasswordPage {
    private fb = inject(FormBuilder);
    private authService = inject(AuthService);
    private router = inject(Router);

    loading = signal<boolean>(false);
    errorMessage = signal<string>('');
    successMessage = signal<string>('');

    isForced = this.authService.requiresPasswordChange;

    passwordForm: FormGroup = this.fb.group({
        currentPassword: ['', Validators.required],
        newPassword: ['', [Validators.required, Validators.minLength(8)]],
        confirmPassword: ['', Validators.required]
    });

    onSubmit(): void {
        if (this.passwordForm.invalid) {
            return;
        }

        const { newPassword, confirmPassword } = this.passwordForm.value;
        if (newPassword !== confirmPassword) {
            this.errorMessage.set('Las contraseñas no coinciden');
            return;
        }

        this.loading.set(true);
        this.errorMessage.set('');

        this.authService.changePassword(this.passwordForm.value).subscribe({
            next: () => {
                this.loading.set(false);
                this.successMessage.set('Contraseña actualizada correctamente');
                setTimeout(() => {
                    this.router.navigate(['/app']);
                }, 2000);
            },
            error: (error) => {
                this.loading.set(false);
                const message = error.error?.message || 'Error al cambiar la contraseña';
                this.errorMessage.set(message);
            }
        });
    }
}
