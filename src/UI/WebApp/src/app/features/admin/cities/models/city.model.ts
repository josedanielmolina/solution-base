export interface City {
    id: number;
    name: string;
    countryId: number;
    countryName: string;
    isActive: boolean;
    createdAt: string;
    updatedAt?: string;
}

export interface CreateCity {
    name: string;
    countryId: number;
}

export interface UpdateCity {
    name: string;
    countryId: number;
    isActive: boolean;
}
