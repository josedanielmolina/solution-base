import { Component, OnInit, inject, signal, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { UserService } from '../../services/user.service';
import { User } from '../../models/user.model';

@Component({
  selector: 'app-user-list-page',
  standalone: true,
  imports: [CommonModule, RouterModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './user-list.page.html',
  styleUrl: './user-list.page.css'
})
export class UserListPage implements OnInit {
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
    this.router.navigate(['/app/users/create']);
  }

  viewUser(id: number): void {
    this.router.navigate(['/app/users', id]);
  }

  editUser(id: number): void {
    this.router.navigate(['/app/users', id, 'edit']);
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
