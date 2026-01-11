import { Component, OnInit, signal, ChangeDetectionStrategy, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../../services/user.service';
import { CreateUser, UpdateUser } from '../../models/user.model';

@Component({
  selector: 'app-user-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './user-form.component.html',
  styleUrl: './user-form.component.css'
})
export class UserFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private userService = inject(UserService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  isEditMode = signal(false);
  userId = signal<number | null>(null);
  loading = signal(false);
  submitting = signal(false);
  error = signal<string | null>(null);

  userForm: FormGroup = this.fb.group({
    firstName: ['', [Validators.required, Validators.maxLength(100)]],
    lastName: ['', [Validators.required, Validators.maxLength(100)]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(8), Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$/)]],
    isActive: [true]
  });

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode.set(true);
      this.userId.set(+id);
      this.loadUser(+id);

      // Remove email and password validators for edit mode
      this.userForm.get('email')?.clearValidators();
      this.userForm.get('password')?.clearValidators();
      this.userForm.get('email')?.updateValueAndValidity();
      this.userForm.get('password')?.updateValueAndValidity();
    }
  }

  loadUser(id: number): void {
    this.loading.set(true);
    this.error.set(null);

    this.userService.getById(id).subscribe({
      next: (user) => {
        this.userForm.patchValue({
          firstName: user.firstName,
          lastName: user.lastName,
          isActive: user.isActive
        });
        this.loading.set(false);
      },
      error: (err) => {
        this.error.set('Failed to load user. Please try again.');
        this.loading.set(false);
        console.error('Error loading user:', err);
      }
    });
  }

  onSubmit(): void {
    if (this.userForm.invalid) {
      Object.keys(this.userForm.controls).forEach(key => {
        this.userForm.get(key)?.markAsTouched();
      });
      return;
    }

    this.submitting.set(true);
    this.error.set(null);

    if (this.isEditMode()) {
      this.updateUser();
    } else {
      this.createUser();
    }
  }

  createUser(): void {
    const userData: CreateUser = this.userForm.value;

    this.userService.create(userData).subscribe({
      next: () => {
        this.submitting.set(false);
        this.router.navigate(['/users']);
      },
      error: (err) => {
        this.error.set('Failed to create user. Please check your data and try again.');
        this.submitting.set(false);
        console.error('Error creating user:', err);
      }
    });
  }

  updateUser(): void {
    const userData: UpdateUser = {
      firstName: this.userForm.value.firstName,
      lastName: this.userForm.value.lastName,
      isActive: this.userForm.value.isActive
    };

    this.userService.update(this.userId()!, userData).subscribe({
      next: () => {
        this.submitting.set(false);
        this.router.navigate(['/users']);
      },
      error: (err) => {
        this.error.set('Failed to update user. Please try again.');
        this.submitting.set(false);
        console.error('Error updating user:', err);
      }
    });
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.userForm.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }

  goBack(): void {
    this.router.navigate(['/users']);
  }
}
