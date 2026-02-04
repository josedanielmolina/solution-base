export interface Country {
    id: number;
    name: string;
    code: string;
    isActive: boolean;
    createdAt: string;
    updatedAt?: string;
}

export interface CreateCountry {
    name: string;
    code: string;
}

export interface UpdateCountry {
    name: string;
    code: string;
    isActive: boolean;
}
