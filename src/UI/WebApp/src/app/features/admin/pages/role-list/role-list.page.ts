import { Component, signal, ChangeDetectionStrategy, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { RoleService } from '@features/admin/services/role.service';
import { Role } from '@features/admin/models/role.model';

@Component({
    selector: 'app-role-list-page',
    standalone: true,
    imports: [CommonModule, RouterLink],
    changeDetection: ChangeDetectionStrategy.OnPush,
    templateUrl: './role-list.page.html',
    styleUrl: './role-list.page.css'
})
export class RoleListPage implements OnInit {
    private roleService = inject(RoleService);

    roles = signal<Role[]>([]);
    loading = signal<boolean>(true);
    errorMessage = signal<string>('');

    ngOnInit(): void {
        this.loadRoles();
    }

    loadRoles(): void {
        this.loading.set(true);
        this.roleService.getAllRoles().subscribe({
            next: (roles) => {
                this.roles.set(roles);
                this.loading.set(false);
            },
            error: (error) => {
                this.loading.set(false);
                this.errorMessage.set(error.error?.message || 'Error al cargar los roles');
            }
        });
    }
}
