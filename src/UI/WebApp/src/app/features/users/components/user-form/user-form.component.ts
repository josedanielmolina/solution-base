import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../../services/user.service';
import { CreateUser, UpdateUser } from '../../models/user.model';

@Component({
  selector: 'app-user-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <div class="user-form-container">
      <div class="form-header">
        <h2>{{ isEditMode() ? 'Edit User' : 'Create User' }}</h2>
        <button class="btn btn-secondary" (click)="goBack()">← Back</button>
      </div>

      @if (loading()) {
        <div class="loading">Loading...</div>
      }

      @if (error()) {
        <div class="error">{{ error() }}</div>
      }

      @if (!loading()) {
        <form [formGroup]="userForm" (ngSubmit)="onSubmit()" class="user-form">
          <div class="form-group">
            <label for="firstName">First Name *</label>
            <input
              id="firstName"
              type="text"
              formControlName="firstName"
              class="form-control"
              [class.invalid]="isFieldInvalid('firstName')"
            />
            @if (isFieldInvalid('firstName')) {
              <div class="error-message">
                @if (userForm.get('firstName')?.hasError('required')) {
                  First name is required
                }
                @if (userForm.get('firstName')?.hasError('maxlength')) {
                  First name cannot exceed 100 characters
                }
              </div>
            }
          </div>

          <div class="form-group">
            <label for="lastName">Last Name *</label>
            <input
              id="lastName"
              type="text"
              formControlName="lastName"
              class="form-control"
              [class.invalid]="isFieldInvalid('lastName')"
            />
            @if (isFieldInvalid('lastName')) {
              <div class="error-message">
                @if (userForm.get('lastName')?.hasError('required')) {
                  Last name is required
                }
                @if (userForm.get('lastName')?.hasError('maxlength')) {
                  Last name cannot exceed 100 characters
                }
              </div>
            }
          </div>

          @if (!isEditMode()) {
            <div class="form-group">
              <label for="email">Email *</label>
              <input
                id="email"
                type="email"
                formControlName="email"
                class="form-control"
                [class.invalid]="isFieldInvalid('email')"
              />
              @if (isFieldInvalid('email')) {
                <div class="error-message">
                  @if (userForm.get('email')?.hasError('required')) {
                    Email is required
                  }
                  @if (userForm.get('email')?.hasError('email')) {
                    Invalid email format
                  }
                </div>
              }
            </div>

            <div class="form-group">
              <label for="password">Password *</label>
              <input
                id="password"
                type="password"
                formControlName="password"
                class="form-control"
                [class.invalid]="isFieldInvalid('password')"
              />
              @if (isFieldInvalid('password')) {
                <div class="error-message">
                  @if (userForm.get('password')?.hasError('required')) {
                    Password is required
                  }
                  @if (userForm.get('password')?.hasError('minlength')) {
                    Password must be at least 8 characters
                  }
                  @if (userForm.get('password')?.hasError('pattern')) {
                    Password must contain uppercase, lowercase, and numbers
                  }
                </div>
              }
            </div>
          }

          <div class="form-group checkbox-group">
            <label>
              <input
                type="checkbox"
                formControlName="isActive"
              />
              <span>Active User</span>
            </label>
          </div>

          <div class="form-actions">
            <button type="button" class="btn btn-secondary" (click)="goBack()">
              Cancel
            </button>
            <button type="submit" class="btn btn-primary" [disabled]="userForm.invalid || submitting()">
              {{ submitting() ? 'Saving...' : (isEditMode() ? 'Update' : 'Create') }}
            </button>
          </div>
        </form>
      }
    </div>
  `,
  styles: [`
    .user-form-container {
      padding: 20px;
      max-width: 600px;
      margin: 0 auto;
    }

    .form-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 30px;
    }

    h2 {
      margin: 0;
      color: #333;
    }

    .user-form {
      background: white;
      padding: 30px;
      border-radius: 8px;
      box-shadow: 0 2px 4px rgba(0,0,0,0.1);
    }

    .form-group {
      margin-bottom: 20px;
    }

    label {
      display: block;
      margin-bottom: 5px;
      font-weight: 500;
      color: #495057;
    }

    .form-control {
      width: 100%;
      padding: 10px;
      border: 1px solid #ced4da;
      border-radius: 4px;
      font-size: 14px;
      transition: border-color 0.3s;
    }

    .form-control:focus {
      outline: none;
      border-color: #007bff;
    }

    .form-control.invalid {
      border-color: #dc3545;
    }

    .error-message {
      color: #dc3545;
      font-size: 12px;
      margin-top: 5px;
    }

    .checkbox-group label {
      display: flex;
      align-items: center;
      cursor: pointer;
    }

    .checkbox-group input[type="checkbox"] {
      width: auto;
      margin-right: 8px;
    }

    .form-actions {
      display: flex;
      gap: 10px;
      justify-content: flex-end;
      margin-top: 30px;
    }

    .btn {
      padding: 10px 20px;
      border: none;
      border-radius: 4px;
      cursor: pointer;
      font-size: 14px;
      transition: all 0.3s;
    }

    .btn-primary {
      background-color: #007bff;
      color: white;
    }

    .btn-primary:hover:not(:disabled) {
      background-color: #0056b3;
    }

    .btn-primary:disabled {
      background-color: #6c757d;
      cursor: not-allowed;
    }

    .btn-secondary {
      background-color: #6c757d;
      color: white;
    }

    .btn-secondary:hover {
      background-color: #5a6268;
    }

    .loading, .error {
      text-align: center;
      padding: 40px;
    }

    .error {
      color: #dc3545;
      background-color: #f8d7da;
      border: 1px solid #f5c6cb;
      border-radius: 4px;
    }
  `]
})
export class UserFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private userService = inject(UserService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  userForm: FormGroup;
  isEditMode = signal(false);
  userId = signal<number | null>(null);
  loading = signal(false);
  submitting = signal(false);
  error = signal<string | null>(null);

  constructor() {
    this.userForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.maxLength(100)]],
      lastName: ['', [Validators.required, Validators.maxLength(100)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8), Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$/)]],
      isActive: [true]
    });
  }

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
