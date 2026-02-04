import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import {
    EstablishmentList,
    Establishment,
    CreateEstablishment,
    UpdateEstablishment,
    Court,
    CreateCourt,
    UpdateCourt,
    Photo,
    CreatePhoto,
    SetSchedules,
    Schedule
} from '../models/establishment.model';

@Injectable({
    providedIn: 'root'
})
export class EstablishmentService {
    private readonly http = inject(HttpClient);
    private readonly baseUrl = `${environment.apiUrl}/api/establishments`;

    // === Establishments ===
    getAll(activeOnly = true): Observable<EstablishmentList[]> {
        return this.http.get<EstablishmentList[]>(`${this.baseUrl}?activeOnly=${activeOnly}`);
    }

    getById(id: number): Observable<Establishment> {
        return this.http.get<Establishment>(`${this.baseUrl}/${id}`);
    }

    search(query: string): Observable<EstablishmentList[]> {
        return this.http.get<EstablishmentList[]>(`${this.baseUrl}/search?q=${encodeURIComponent(query)}`);
    }

    create(data: CreateEstablishment): Observable<Establishment> {
        return this.http.post<Establishment>(this.baseUrl, data);
    }

    update(id: number, data: UpdateEstablishment): Observable<Establishment> {
        return this.http.put<Establishment>(`${this.baseUrl}/${id}`, data);
    }

    delete(id: number): Observable<void> {
        return this.http.delete<void>(`${this.baseUrl}/${id}`);
    }

    // === Photos ===
    addPhoto(establishmentId: number, photo: CreatePhoto): Observable<Photo> {
        return this.http.post<Photo>(`${this.baseUrl}/${establishmentId}/photos`, photo);
    }

    removePhoto(establishmentId: number, photoId: number): Observable<void> {
        return this.http.delete<void>(`${this.baseUrl}/${establishmentId}/photos/${photoId}`);
    }

    // === Schedules ===
    setSchedules(establishmentId: number, data: SetSchedules): Observable<Schedule[]> {
        return this.http.put<Schedule[]>(`${this.baseUrl}/${establishmentId}/schedules`, data);
    }

    // === Courts ===
    getCourts(establishmentId: number, activeOnly = true): Observable<Court[]> {
        return this.http.get<Court[]>(`${this.baseUrl}/${establishmentId}/courts?activeOnly=${activeOnly}`);
    }

    getCourt(establishmentId: number, id: number): Observable<Court> {
        return this.http.get<Court>(`${this.baseUrl}/${establishmentId}/courts/${id}`);
    }

    createCourt(establishmentId: number, data: CreateCourt): Observable<Court> {
        return this.http.post<Court>(`${this.baseUrl}/${establishmentId}/courts`, data);
    }

    updateCourt(establishmentId: number, id: number, data: UpdateCourt): Observable<Court> {
        return this.http.put<Court>(`${this.baseUrl}/${establishmentId}/courts/${id}`, data);
    }

    deleteCourt(establishmentId: number, id: number): Observable<void> {
        return this.http.delete<void>(`${this.baseUrl}/${establishmentId}/courts/${id}`);
    }

    addCourtPhoto(establishmentId: number, courtId: number, photo: CreatePhoto): Observable<Photo> {
        return this.http.post<Photo>(`${this.baseUrl}/${establishmentId}/courts/${courtId}/photos`, photo);
    }

    removeCourtPhoto(establishmentId: number, courtId: number, photoId: number): Observable<void> {
        return this.http.delete<void>(`${this.baseUrl}/${establishmentId}/courts/${courtId}/photos/${photoId}`);
    }
}
