export interface EventListDto {
    id: number;
    publicId: string;
    name: string;
    description?: string;
    organizerId?: number;
    organizerName?: string;
    startDate: Date;
    endDate: Date;
    isActive: boolean;
    establishmentsCount: number;
    adminsCount: number;
    createdAt: Date;
}

export interface EventDto {
    id: number;
    publicId: string;
    name: string;
    description?: string;
    organizerId?: number;
    organizerName?: string;
    contactPhone?: string;
    startDate: Date;
    endDate: Date;
    posterVertical?: string;
    posterHorizontal?: string;
    rulesPdf?: string;
    whatsApp?: string;
    facebook?: string;
    instagram?: string;
    isActive: boolean;
    createdAt: Date;
    updatedAt?: Date;
    establishments: EventEstablishmentDto[];
    admins: EventAdminDto[];
}

export interface CreateEventDto {
    name: string;
    description?: string;
    organizerId?: number;
    contactPhone?: string;
    startDate: Date;
    endDate: Date;
    whatsApp?: string;
    facebook?: string;
    instagram?: string;
}

export interface UpdateEventDto {
    name: string;
    description?: string;
    organizerId?: number | null;
    contactPhone?: string;
    startDate: Date;
    endDate: Date;
    whatsApp?: string;
    facebook?: string;
    instagram?: string;
}

export interface EventEstablishmentDto {
    id: number;
    establishmentId: number;
    establishmentName: string;
    city?: string;
    createdAt: Date;
}

export interface AddEventEstablishmentDto {
    establishmentId: number;
}

export interface EventAdminDto {
    id: number;
    userId: number;
    userName: string;
    userEmail: string;
    createdAt: Date;
}

export interface InviteAdminDto {
    email: string;
}

export interface EventInvitationDto {
    id: number;
    email: string;
    token: string;
    expiresAt: Date;
    acceptedAt?: Date;
    createdAt: Date;
    isExpired: boolean;
    isAccepted: boolean;
}

export interface UploadPosterDto {
    imageData: string;
}

export interface UploadRulesPdfDto {
    pdfData: string;
}
