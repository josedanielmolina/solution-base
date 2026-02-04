export interface User {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  document?: string;
  phone?: string;
  isActive: boolean;
  isEmailVerified: boolean;
  requiresPasswordChange: boolean;
  lastLoginAt?: string;
  createdAt: string;
  updatedAt?: string;
  roles?: string[];
}

export interface CreateUser {
  firstName: string;
  lastName: string;
  email: string;
  document?: string;
  phone?: string;
  password: string;
  roleIds?: number[];
}

export interface UpdateUser {
  firstName: string;
  lastName: string;
  document?: string;
  phone?: string;
  isActive: boolean;
}

export interface UpdateProfile {
  firstName: string;
  lastName: string;
  document?: string;
  phone?: string;
}

export interface AssignRoles {
  roleIds: number[];
}

