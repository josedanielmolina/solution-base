import { Component, signal, ChangeDetectionStrategy, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '@core/services/auth.service';
import { UserService } from '@features/users/services/user.service';

@Component({
    selector: 'app-profile-page',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule],
    changeDetection: ChangeDetectionStrategy.OnPush,
    templateUrl: './profile.page.html',
    styleUrl: './profile.page.css'
})
export class ProfilePage implements OnInit {
    private fb = inject(FormBuilder);
    private authService = inject(AuthService);
    private userService = inject(UserService);

    loading = signal<boolean>(false);
    errorMessage = signal<string>('');
    successMessage = signal<string>('');

    currentUser = this.authService.currentUser;

    profileForm: FormGroup = this.fb.group({
        firstName: ['', Validators.required],
        lastName: ['', Validators.required],
        document: [''],
        phone: ['']
    });

    ngOnInit(): void {
        const user = this.currentUser();
        if (user) {
            this.profileForm.patchValue({
                firstName: user.firstName,
                lastName: user.lastName
            });
        }
    }

    onSubmit(): void {
        if (this.profileForm.invalid) {
            return;
        }

        this.loading.set(true);
        this.errorMessage.set('');
        this.successMessage.set('');

        this.userService.updateProfile(this.profileForm.value).subscribe({
            next: () => {
                this.loading.set(false);
                this.successMessage.set('Perfil actualizado correctamente');
                // Refresh current user
                this.authService.getCurrentUser().subscribe();
            },
            error: (error) => {
                this.loading.set(false);
                const message = error.error?.message || 'Error al actualizar el perfil';
                this.errorMessage.set(message);
            }
        });
    }
}
