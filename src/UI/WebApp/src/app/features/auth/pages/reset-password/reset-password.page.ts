import { Component, signal, ChangeDetectionStrategy, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink, ActivatedRoute } from '@angular/router';
import { AuthService } from '@core/services/auth.service';

@Component({
    selector: 'app-reset-password-page',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule, RouterLink],
    changeDetection: ChangeDetectionStrategy.OnPush,
    templateUrl: './reset-password.page.html',
    styleUrl: './reset-password.page.css'
})
export class ResetPasswordPage implements OnInit {
    private fb = inject(FormBuilder);
    private authService = inject(AuthService);
    private router = inject(Router);
    private route = inject(ActivatedRoute);

    loading = signal<boolean>(false);
    errorMessage = signal<string>('');
    successMessage = signal<string>('');

    resetForm: FormGroup = this.fb.group({
        email: ['', [Validators.required, Validators.email]],
        code: ['', [Validators.required, Validators.pattern(/^\d{6}$/)]],
        newPassword: ['', [Validators.required, Validators.minLength(8)]],
        confirmPassword: ['', Validators.required]
    });

    ngOnInit(): void {
        const email = this.route.snapshot.queryParamMap.get('email');
        if (email) {
            this.resetForm.patchValue({ email });
        }
    }

    onSubmit(): void {
        if (this.resetForm.invalid) {
            return;
        }

        const { newPassword, confirmPassword } = this.resetForm.value;
        if (newPassword !== confirmPassword) {
            this.errorMessage.set('Las contraseñas no coinciden');
            return;
        }

        this.loading.set(true);
        this.errorMessage.set('');

        this.authService.resetPassword(this.resetForm.value).subscribe({
            next: () => {
                this.loading.set(false);
                this.successMessage.set('Contraseña restablecida correctamente');
                setTimeout(() => {
                    this.router.navigate(['/login']);
                }, 2000);
            },
            error: (error) => {
                this.loading.set(false);
                const message = error.error?.message || 'Código inválido o expirado';
                this.errorMessage.set(message);
            }
        });
    }
}
