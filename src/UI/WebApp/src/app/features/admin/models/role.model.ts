export interface Role {
    id: number;
    name: string;
    description?: string;
}

export interface RoleWithPermissions {
    id: number;
    name: string;
    description?: string;
    permissions: Permission[];
}

export interface Permission {
    id: number;
    name: string;
    description?: string;
    module: string;
}

export interface UpdateRolePermissions {
    permissionIds: number[];
}
