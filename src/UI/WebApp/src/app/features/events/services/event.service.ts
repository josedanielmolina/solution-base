import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import {
    EventListDto,
    EventDto,
    CreateEventDto,
    UpdateEventDto,
    EventEstablishmentDto,
    AddEventEstablishmentDto,
    EventAdminDto,
    InviteAdminDto,
    EventInvitationDto,
    UploadPosterDto,
    UploadRulesPdfDto
} from '../models/event.model';

@Injectable({
    providedIn: 'root'
})
export class EventService {
    private http = inject(HttpClient);
    private baseUrl = `${environment.apiUrl}/api/events`;

    // ============================================================================
    // EVENTS
    // ============================================================================

    getMyEvents(): Observable<EventListDto[]> {
        return this.http.get<EventListDto[]>(this.baseUrl);
    }

    getEvent(publicId: string): Observable<EventDto> {
        return this.http.get<EventDto>(`${this.baseUrl}/${publicId}`);
    }

    createEvent(dto: CreateEventDto): Observable<EventDto> {
        return this.http.post<EventDto>(this.baseUrl, dto);
    }

    updateEvent(publicId: string, dto: UpdateEventDto): Observable<void> {
        return this.http.put<void>(`${this.baseUrl}/${publicId}`, dto);
    }

    deleteEvent(publicId: string): Observable<void> {
        return this.http.delete<void>(`${this.baseUrl}/${publicId}`);
    }

    // ============================================================================
    // POSTERS & PDF
    // ============================================================================

    uploadPoster(publicId: string, type: 'vertical' | 'horizontal', dto: UploadPosterDto): Observable<void> {
        return this.http.post<void>(`${this.baseUrl}/${publicId}/poster-${type}`, dto);
    }

    removePoster(publicId: string, type: 'vertical' | 'horizontal'): Observable<void> {
        return this.http.delete<void>(`${this.baseUrl}/${publicId}/poster/${type}`);
    }

    uploadRulesPdf(publicId: string, dto: UploadRulesPdfDto): Observable<void> {
        return this.http.post<void>(`${this.baseUrl}/${publicId}/rules-pdf`, dto);
    }

    removeRulesPdf(publicId: string): Observable<void> {
        return this.http.delete<void>(`${this.baseUrl}/${publicId}/rules-pdf`);
    }

    // ============================================================================
    // ESTABLISHMENTS
    // ============================================================================

    getEstablishments(publicId: string): Observable<EventEstablishmentDto[]> {
        return this.http.get<EventEstablishmentDto[]>(`${this.baseUrl}/${publicId}/establishments`);
    }

    addEstablishment(publicId: string, dto: AddEventEstablishmentDto): Observable<EventEstablishmentDto> {
        return this.http.post<EventEstablishmentDto>(`${this.baseUrl}/${publicId}/establishments`, dto);
    }

    removeEstablishment(publicId: string, establishmentId: number): Observable<void> {
        return this.http.delete<void>(`${this.baseUrl}/${publicId}/establishments/${establishmentId}`);
    }

    searchAvailableEstablishments(publicId: string, query?: string): Observable<any[]> {
        const params = query ? `?q=${encodeURIComponent(query)}` : '';
        return this.http.get<any[]>(`${this.baseUrl}/${publicId}/establishments/search${params}`);
    }

    // ============================================================================
    // ADMINS
    // ============================================================================

    getAdmins(publicId: string): Observable<EventAdminDto[]> {
        return this.http.get<EventAdminDto[]>(`${this.baseUrl}/${publicId}/admins`);
    }

    inviteAdmin(publicId: string, dto: InviteAdminDto): Observable<EventInvitationDto> {
        return this.http.post<EventInvitationDto>(`${this.baseUrl}/${publicId}/admins/invite`, dto);
    }

    removeAdmin(publicId: string, userId: number): Observable<void> {
        return this.http.delete<void>(`${this.baseUrl}/${publicId}/admins/${userId}`);
    }

    getPendingInvitations(publicId: string): Observable<EventInvitationDto[]> {
        return this.http.get<EventInvitationDto[]>(`${this.baseUrl}/${publicId}/invitations`);
    }

    // ============================================================================
    // INVITATIONS (global endpoint)
    // ============================================================================

    acceptInvitation(token: string): Observable<{ message: string }> {
        return this.http.post<{ message: string }>(`${environment.apiUrl}/api/invitations/${token}/accept`, {});
    }
}
