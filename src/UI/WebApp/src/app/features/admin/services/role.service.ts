import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '@environments/environment';
import { Role, RoleWithPermissions, Permission, UpdateRolePermissions } from '@features/admin/models/role.model';

@Injectable({
    providedIn: 'root'
})
export class RoleService {
    private http = inject(HttpClient);
    private apiUrl = environment.apiUrl;

    // Roles
    getAllRoles(): Observable<Role[]> {
        return this.http.get<Role[]>(`${this.apiUrl}/api/roles`);
    }

    getRoleById(id: number): Observable<RoleWithPermissions> {
        return this.http.get<RoleWithPermissions>(`${this.apiUrl}/api/roles/${id}`);
    }

    updateRolePermissions(roleId: number, permissions: UpdateRolePermissions): Observable<void> {
        return this.http.put<void>(`${this.apiUrl}/api/roles/${roleId}/permissions`, permissions);
    }

    // Permissions
    getAllPermissions(): Observable<Permission[]> {
        return this.http.get<Permission[]>(`${this.apiUrl}/api/permissions`);
    }

    getPermissionsByModule(module: string): Observable<Permission[]> {
        return this.http.get<Permission[]>(`${this.apiUrl}/api/permissions/module/${module}`);
    }
}
