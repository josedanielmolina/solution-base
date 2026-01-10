import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { UserService } from '../../services/user.service';
import { User } from '../../models/user.model';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <div class="user-list">
      <div class="header">
        <h2>Users Management</h2>
        <button class="btn btn-primary" (click)="navigateToCreate()">
          <span class="icon">+</span> Create User
        </button>
      </div>

      @if (loading()) {
        <div class="loading">Loading users...</div>
      }

      @if (error()) {
        <div class="error">{{ error() }}</div>
      }

      @if (!loading() && users().length === 0) {
        <div class="empty-state">
          <p>No users found. Create your first user!</p>
        </div>
      }

      @if (!loading() && users().length > 0) {
        <div class="table-container">
          <table class="user-table">
            <thead>
              <tr>
                <th>ID</th>
                <th>Name</th>
                <th>Email</th>
                <th>Status</th>
                <th>Created At</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              @for (user of users(); track user.id) {
                <tr>
                  <td>{{ user.id }}</td>
                  <td>{{ user.firstName }} {{ user.lastName }}</td>
                  <td>{{ user.email }}</td>
                  <td>
                    <span [class]="user.isActive ? 'status-active' : 'status-inactive'">
                      {{ user.isActive ? 'Active' : 'Inactive' }}
                    </span>
                  </td>
                  <td>{{ user.createdAt | date: 'short' }}</td>
                  <td class="actions">
                    <button class="btn btn-sm btn-info" (click)="viewUser(user.id)" title="View">
                      👁️
                    </button>
                    <button class="btn btn-sm btn-warning" (click)="editUser(user.id)" title="Edit">
                      ✏️
                    </button>
                    <button class="btn btn-sm btn-danger" (click)="deleteUser(user.id)" title="Delete">
                      🗑️
                    </button>
                  </td>
                </tr>
              }
            </tbody>
          </table>
        </div>
      }
    </div>
  `,
  styles: [`
    .user-list {
      padding: 20px;
      max-width: 1200px;
      margin: 0 auto;
    }

    .header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 20px;
    }

    h2 {
      margin: 0;
      color: #333;
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

    .btn-primary:hover {
      background-color: #0056b3;
    }

    .btn-sm {
      padding: 5px 10px;
      font-size: 12px;
      margin: 0 2px;
    }

    .btn-info {
      background-color: #17a2b8;
      color: white;
    }

    .btn-warning {
      background-color: #ffc107;
      color: #333;
    }

    .btn-danger {
      background-color: #dc3545;
      color: white;
    }

    .icon {
      font-size: 18px;
      margin-right: 5px;
    }

    .loading, .error, .empty-state {
      text-align: center;
      padding: 40px;
      color: #666;
    }

    .error {
      color: #dc3545;
      background-color: #f8d7da;
      border: 1px solid #f5c6cb;
      border-radius: 4px;
    }

    .table-container {
      overflow-x: auto;
      background: white;
      border-radius: 8px;
      box-shadow: 0 2px 4px rgba(0,0,0,0.1);
    }

    .user-table {
      width: 100%;
      border-collapse: collapse;
    }

    .user-table th,
    .user-table td {
      padding: 12px;
      text-align: left;
      border-bottom: 1px solid #dee2e6;
    }

    .user-table th {
      background-color: #f8f9fa;
      font-weight: 600;
      color: #495057;
    }

    .user-table tbody tr:hover {
      background-color: #f8f9fa;
    }

    .actions {
      white-space: nowrap;
    }

    .status-active {
      color: #28a745;
      font-weight: 500;
    }

    .status-inactive {
      color: #6c757d;
      font-weight: 500;
    }
  `]
})
export class UserListComponent implements OnInit {
  private userService = inject(UserService);
  private router = inject(Router);

  users = signal<User[]>([]);
  loading = signal(false);
  error = signal<string | null>(null);

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.loading.set(true);
    this.error.set(null);

    this.userService.getAll().subscribe({
      next: (users) => {
        this.users.set(users);
        this.loading.set(false);
      },
      error: (err) => {
        this.error.set('Failed to load users. Please try again.');
        this.loading.set(false);
        console.error('Error loading users:', err);
      }
    });
  }

  navigateToCreate(): void {
    this.router.navigate(['/users/create']);
  }

  viewUser(id: number): void {
    this.router.navigate(['/users', id]);
  }

  editUser(id: number): void {
    this.router.navigate(['/users', id, 'edit']);
  }

  deleteUser(id: number): void {
    if (confirm('Are you sure you want to delete this user?')) {
      this.userService.delete(id).subscribe({
        next: () => {
          this.loadUsers();
        },
        error: (err) => {
          this.error.set('Failed to delete user. Please try again.');
          console.error('Error deleting user:', err);
        }
      });
    }
  }
}
