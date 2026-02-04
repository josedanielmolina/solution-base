import { Component, OnInit, inject, signal, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { UserService } from '../../services/user.service';
import { User } from '../../models/user.model';
import { RoleService } from '@features/admin/services/role.service';
import { Role } from '@features/admin/models/role.model';

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
  private roleService = inject(RoleService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  user = signal<User | null>(null);
  loading = signal(false);
  error = signal<string | null>(null);

  // Role management
  allRoles = signal<Role[]>([]);
  selectedRoleIds = signal<Set<number>>(new Set());
  savingRoles = signal(false);
  rolesSuccess = signal<string | null>(null);

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.loadAllRoles(+id);
    }
  }

  loadUser(id: number): void {
    this.loading.set(true);
    this.error.set(null);

    this.userService.getById(id).subscribe({
      next: (user) => {
        this.user.set(user);
        // Initialize selected roles based on user's current roles
        this.initializeSelectedRoles(user);
        this.loading.set(false);
      },
      error: (err) => {
        this.error.set('Error al cargar los detalles del usuario.');
        this.loading.set(false);
        console.error('Error loading user:', err);
      }
    });
  }

  loadAllRoles(userId: number): void {
    this.roleService.getAllRoles().subscribe({
      next: (roles) => {
        this.allRoles.set(roles);
        // After loading roles, load the user
        this.loadUser(userId);
      },
      error: (err) => {
        console.error('Error loading roles:', err);
        // Still try to load user even if roles fail
        this.loadUser(userId);
      }
    });
  }

  initializeSelectedRoles(user: User): void {
    // Match user's role names to role IDs
    const userRoleNames = user.roles || [];
    const allRoles = this.allRoles();
    const selectedIds = new Set<number>();

    for (const role of allRoles) {
      if (userRoleNames.includes(role.name)) {
        selectedIds.add(role.id);
      }
    }
    this.selectedRoleIds.set(selectedIds);
  }

  isRoleSelected(roleId: number): boolean {
    return this.selectedRoleIds().has(roleId);
  }

  toggleRole(roleId: number): void {
    const currentSelectedRoles = new Set(this.selectedRoleIds());
    if (currentSelectedRoles.has(roleId)) {
      currentSelectedRoles.delete(roleId);
    } else {
      currentSelectedRoles.add(roleId);
    }
    this.selectedRoleIds.set(currentSelectedRoles);
  }

  saveRoles(): void {
    const userId = this.user()?.id;
    if (!userId) return;

    this.savingRoles.set(true);
    this.rolesSuccess.set(null);
    this.error.set(null);

    const roleIds = Array.from(this.selectedRoleIds());

    this.userService.assignRoles(userId, { roleIds }).subscribe({
      next: () => {
        this.savingRoles.set(false);
        this.rolesSuccess.set('Roles actualizados correctamente');
        // Reload user to get updated roles
        this.loadUser(userId);
        setTimeout(() => this.rolesSuccess.set(null), 3000);
      },
      error: (err) => {
        this.savingRoles.set(false);
        this.error.set('Error al actualizar los roles');
        console.error('Error saving roles:', err);
      }
    });
  }

  editUser(): void {
    this.router.navigate(['/app/users', this.user()!.id, 'edit']);
  }

  deleteUser(): void {
    if (confirm('¿Está seguro de que desea eliminar este usuario?')) {
      this.userService.delete(this.user()!.id).subscribe({
        next: () => {
          this.router.navigate(['/app/users']);
        },
        error: (err) => {
          this.error.set('Error al eliminar el usuario.');
          console.error('Error deleting user:', err);
        }
      });
    }
  }

  goBack(): void {
    this.router.navigate(['/app/users']);
  }
}
