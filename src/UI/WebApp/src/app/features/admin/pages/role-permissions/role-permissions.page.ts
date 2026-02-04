import { Component, signal, ChangeDetectionStrategy, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { RoleService } from '@features/admin/services/role.service';
import { RoleWithPermissions, Permission } from '@features/admin/models/role.model';

@Component({
    selector: 'app-role-permissions-page',
    standalone: true,
    imports: [CommonModule, RouterLink, FormsModule],
    changeDetection: ChangeDetectionStrategy.OnPush,
    templateUrl: './role-permissions.page.html',
    styleUrl: './role-permissions.page.css'
})
export class RolePermissionsPage implements OnInit {
    private roleService = inject(RoleService);
    private route = inject(ActivatedRoute);
    private router = inject(Router);

    role = signal<RoleWithPermissions | null>(null);
    allPermissions = signal<Permission[]>([]);
    selectedPermissionIds = signal<Set<number>>(new Set());
    groupedPermissions = signal<Map<string, Permission[]>>(new Map());

    loading = signal<boolean>(true);
    saving = signal<boolean>(false);
    errorMessage = signal<string>('');
    successMessage = signal<string>('');

    ngOnInit(): void {
        const roleId = this.route.snapshot.paramMap.get('id');
        if (roleId) {
            this.loadData(parseInt(roleId, 10));
        }
    }

    loadData(roleId: number): void {
        this.loading.set(true);

        // Load role with current permissions
        this.roleService.getRoleById(roleId).subscribe({
            next: (role) => {
                this.role.set(role);
                const selectedIds = new Set(role.permissions.map(p => p.id));
                this.selectedPermissionIds.set(selectedIds);

                // Load all permissions
                this.roleService.getAllPermissions().subscribe({
                    next: (permissions) => {
                        this.allPermissions.set(permissions);
                        this.groupPermissionsByModule(permissions);
                        this.loading.set(false);
                    },
                    error: (error) => {
                        this.loading.set(false);
                        this.errorMessage.set(error.error?.message || 'Error al cargar permisos');
                    }
                });
            },
            error: (error) => {
                this.loading.set(false);
                this.errorMessage.set(error.error?.message || 'Error al cargar el rol');
            }
        });
    }

    groupPermissionsByModule(permissions: Permission[]): void {
        const grouped = new Map<string, Permission[]>();
        permissions.forEach(p => {
            const module = p.module || 'General';
            if (!grouped.has(module)) {
                grouped.set(module, []);
            }
            grouped.get(module)!.push(p);
        });
        this.groupedPermissions.set(grouped);
    }

    isSelected(permissionId: number): boolean {
        return this.selectedPermissionIds().has(permissionId);
    }

    togglePermission(permissionId: number): void {
        const current = new Set(this.selectedPermissionIds());
        if (current.has(permissionId)) {
            current.delete(permissionId);
        } else {
            current.add(permissionId);
        }
        this.selectedPermissionIds.set(current);
    }

    toggleAllInModule(module: string, checked: boolean): void {
        const current = new Set(this.selectedPermissionIds());
        const modulePermissions = this.groupedPermissions().get(module) || [];

        modulePermissions.forEach(p => {
            if (checked) {
                current.add(p.id);
            } else {
                current.delete(p.id);
            }
        });

        this.selectedPermissionIds.set(current);
    }

    isModuleFullySelected(module: string): boolean {
        const modulePermissions = this.groupedPermissions().get(module) || [];
        return modulePermissions.every(p => this.selectedPermissionIds().has(p.id));
    }

    save(): void {
        const role = this.role();
        if (!role) return;

        this.saving.set(true);
        this.errorMessage.set('');
        this.successMessage.set('');

        const permissionIds = Array.from(this.selectedPermissionIds());

        this.roleService.updateRolePermissions(role.id, { permissionIds }).subscribe({
            next: () => {
                this.saving.set(false);
                this.successMessage.set('Permisos actualizados correctamente');
            },
            error: (error) => {
                this.saving.set(false);
                this.errorMessage.set(error.error?.message || 'Error al guardar permisos');
            }
        });
    }
}
