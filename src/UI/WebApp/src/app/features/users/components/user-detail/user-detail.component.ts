import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { UserService } from '../../services/user.service';
import { User } from '../../models/user.model';

@Component({
  selector: 'app-user-detail',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <div class="user-detail-container">
      <div class="header">
        <h2>User Details</h2>
        <button class="btn btn-secondary" (click)="goBack()">← Back</button>
      </div>

      @if (loading()) {
        <div class="loading">Loading user details...</div>
      }

      @if (error()) {
        <div class="error">{{ error() }}</div>
      }

      @if (!loading() && user()) {
        <div class="user-detail-card">
          <div class="detail-section">
            <h3>Personal Information</h3>
            <div class="detail-grid">
              <div class="detail-item">
                <label>ID:</label>
                <span>{{ user()!.id }}</span>
              </div>
              <div class="detail-item">
                <label>First Name:</label>
                <span>{{ user()!.firstName }}</span>
              </div>
              <div class="detail-item">
                <label>Last Name:</label>
                <span>{{ user()!.lastName }}</span>
              </div>
              <div class="detail-item">
                <label>Email:</label>
                <span>{{ user()!.email }}</span>
              </div>
            </div>
          </div>

          <div class="detail-section">
            <h3>Status & Activity</h3>
            <div class="detail-grid">
              <div class="detail-item">
                <label>Status:</label>
                <span [class]="user()!.isActive ? 'status-active' : 'status-inactive'">
                  {{ user()!.isActive ? 'Active' : 'Inactive' }}
                </span>
              </div>
              <div class="detail-item">
                <label>Email Verified:</label>
                <span [class]="user()!.isEmailVerified ? 'status-active' : 'status-inactive'">
                  {{ user()!.isEmailVerified ? 'Yes' : 'No' }}
                </span>
              </div>
              <div class="detail-item">
                <label>Created At:</label>
                <span>{{ user()!.createdAt | date: 'medium' }}</span>
              </div>
              @if (user()!.updatedAt) {
                <div class="detail-item">
                  <label>Updated At:</label>
                  <span>{{ user()!.updatedAt | date: 'medium' }}</span>
                </div>
              }
              @if (user()!.lastLoginAt) {
                <div class="detail-item">
                  <label>Last Login:</label>
                  <span>{{ user()!.lastLoginAt | date: 'medium' }}</span>
                </div>
              }
            </div>
          </div>

          <div class="actions">
            <button class="btn btn-warning" (click)="editUser()">
              ✏️ Edit User
            </button>
            <button class="btn btn-danger" (click)="deleteUser()">
              🗑️ Delete User
            </button>
          </div>
        </div>
      }
    </div>
  `,
  styles: [`
    .user-detail-container {
      padding: 20px;
      max-width: 800px;
      margin: 0 auto;
    }

    .header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 30px;
    }

    h2 {
      margin: 0;
      color: #333;
    }

    h3 {
      color: #495057;
      margin-bottom: 15px;
      font-size: 18px;
      border-bottom: 2px solid #007bff;
      padding-bottom: 8px;
    }

    .user-detail-card {
      background: white;
      padding: 30px;
      border-radius: 8px;
      box-shadow: 0 2px 4px rgba(0,0,0,0.1);
    }

    .detail-section {
      margin-bottom: 30px;
    }

    .detail-grid {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
      gap: 20px;
    }

    .detail-item {
      display: flex;
      flex-direction: column;
      gap: 5px;
    }

    .detail-item label {
      font-weight: 600;
      color: #6c757d;
      font-size: 14px;
    }

    .detail-item span {
      color: #333;
      font-size: 16px;
    }

    .status-active {
      color: #28a745;
      font-weight: 600;
    }

    .status-inactive {
      color: #6c757d;
      font-weight: 600;
    }

    .actions {
      display: flex;
      gap: 10px;
      justify-content: flex-end;
      padding-top: 20px;
      border-top: 1px solid #dee2e6;
    }

    .btn {
      padding: 10px 20px;
      border: none;
      border-radius: 4px;
      cursor: pointer;
      font-size: 14px;
      transition: all 0.3s;
    }

    .btn-secondary {
      background-color: #6c757d;
      color: white;
    }

    .btn-secondary:hover {
      background-color: #5a6268;
    }

    .btn-warning {
      background-color: #ffc107;
      color: #333;
    }

    .btn-warning:hover {
      background-color: #e0a800;
    }

    .btn-danger {
      background-color: #dc3545;
      color: white;
    }

    .btn-danger:hover {
      background-color: #c82333;
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
export class UserDetailComponent implements OnInit {
  private userService = inject(UserService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  user = signal<User | null>(null);
  loading = signal(false);
  error = signal<string | null>(null);

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.loadUser(+id);
    }
  }

  loadUser(id: number): void {
    this.loading.set(true);
    this.error.set(null);

    this.userService.getById(id).subscribe({
      next: (user) => {
        this.user.set(user);
        this.loading.set(false);
      },
      error: (err) => {
        this.error.set('Failed to load user details. Please try again.');
        this.loading.set(false);
        console.error('Error loading user:', err);
      }
    });
  }

  editUser(): void {
    this.router.navigate(['/users', this.user()!.id, 'edit']);
  }

  deleteUser(): void {
    if (confirm('Are you sure you want to delete this user?')) {
      this.userService.delete(this.user()!.id).subscribe({
        next: () => {
          this.router.navigate(['/users']);
        },
        error: (err) => {
          this.error.set('Failed to delete user. Please try again.');
          console.error('Error deleting user:', err);
        }
      });
    }
  }

  goBack(): void {
    this.router.navigate(['/users']);
  }
}
