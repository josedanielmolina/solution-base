import { Component, OnInit, inject, signal, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { UserService } from '../../services/user.service';
import { User } from '../../models/user.model';

@Component({
  selector: 'app-user-detail-page',
  standalone: true,
  imports: [CommonModule, RouterModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './user-detail.page.html',
  styleUrl: './user-detail.page.css'
})
export class UserDetailPage implements OnInit {
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
    this.router.navigate(['/app/users', this.user()!.id, 'edit']);
  }

  deleteUser(): void {
    if (confirm('Are you sure you want to delete this user?')) {
      this.userService.delete(this.user()!.id).subscribe({
        next: () => {
          this.router.navigate(['/app/users']);
        },
        error: (err) => {
          this.error.set('Failed to delete user. Please try again.');
          console.error('Error deleting user:', err);
        }
      });
    }
  }

  goBack(): void {
    this.router.navigate(['/app/users']);
  }
}
