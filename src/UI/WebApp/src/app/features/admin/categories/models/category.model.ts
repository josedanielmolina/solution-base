export interface Category {
    id: number;
    name: string;
    gender: string;
    countryId: number;
    countryName: string;
    isActive: boolean;
    createdAt: string;
    updatedAt?: string;
}

export type Gender = 'Male' | 'Female' | 'Mixed';

export const GENDERS: { value: number; label: string }[] = [
    { value: 1, label: 'Masculino' },
    { value: 2, label: 'Femenino' },
    { value: 3, label: 'Mixto' }
];

export interface CreateCategory {
    name: string;
    gender: number;
    countryId: number;
}

export interface UpdateCategory {
    name: string;
    gender: number;
    countryId: number;
    isActive: boolean;
}
