// === List DTOs ===
export interface EstablishmentList {
    id: number;
    name: string;
    countryId: number;
    countryName: string;
    cityId: number;
    cityName: string;
    address: string;
    courtsCount: number;
    isActive: boolean;
}

// === Detail DTO ===
export interface Establishment {
    id: number;
    name: string;
    countryId: number;
    countryName: string;
    cityId: number;
    cityName: string;
    address: string;
    googleMapsUrl?: string;
    phoneLandline?: string;
    phoneMobile?: string;
    logo?: string;
    scheduleType: number;
    isActive: boolean;
    createdAt: string;
    updatedAt?: string;
    courts: Court[];
    photos: Photo[];
    schedules: Schedule[];
}

// === Create/Update DTOs ===
export interface CreateEstablishment {
    name: string;
    countryId: number;
    cityId: number;
    address: string;
    googleMapsUrl?: string;
    phoneLandline?: string;
    phoneMobile?: string;
    logo?: string;
    scheduleType: number;
}

export interface UpdateEstablishment extends CreateEstablishment {
    isActive: boolean;
}

// === Court ===
export interface Court {
    id: number;
    establishmentId: number;
    name: string;
    courtType: number;
    courtTypeName: string;
    isActive: boolean;
    photos: Photo[];
}

export interface CreateCourt {
    name: string;
    courtType: number;
}

export interface UpdateCourt extends CreateCourt {
    isActive: boolean;
}

// === Photo ===
export interface Photo {
    id: number;
    imageData: string;
    displayOrder: number;
}

export interface CreatePhoto {
    imageData: string;
    displayOrder: number;
}

// === Schedule ===
export interface Schedule {
    id: number;
    dayOfWeek: number;
    dayName: string;
    openTime: string;
    closeTime: string;
    blockNumber: number;
}

export interface SetSchedule {
    dayOfWeek: number;
    openTime: string;
    closeTime: string;
    blockNumber: number;
}

export interface SetSchedules {
    scheduleType: number;
    schedules: SetSchedule[];
}

// === Constants ===
export const SCHEDULE_TYPES = [
    { value: 1, label: 'Continuo' },
    { value: 2, label: 'Por Bloques' }
];

export const COURT_TYPES = [
    { value: 1, label: 'Indoor' },
    { value: 2, label: 'Outdoor' }
];

export const DAYS_OF_WEEK = [
    { value: 1, label: 'Lunes' },
    { value: 2, label: 'Martes' },
    { value: 3, label: 'Miércoles' },
    { value: 4, label: 'Jueves' },
    { value: 5, label: 'Viernes' },
    { value: 6, label: 'Sábado' },
    { value: 7, label: 'Domingo' }
];
