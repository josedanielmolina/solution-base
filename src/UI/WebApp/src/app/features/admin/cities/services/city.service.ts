import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '@environments/environment';
import { City, CreateCity, UpdateCity } from '../models/city.model';

@Injectable({ providedIn: 'root' })
export class CityService {
    private http = inject(HttpClient);
    private baseUrl = `${environment.apiUrl}/api/cities`;

    getAll(activeOnly = true): Observable<City[]> {
        return this.http.get<City[]>(`${this.baseUrl}?activeOnly=${activeOnly}`);
    }

    getByCountry(countryId: number): Observable<City[]> {
        return this.http.get<City[]>(`${this.baseUrl}/by-country/${countryId}`);
    }

    getById(id: number): Observable<City> {
        return this.http.get<City>(`${this.baseUrl}/${id}`);
    }

    create(data: CreateCity): Observable<City> {
        return this.http.post<City>(this.baseUrl, data);
    }

    update(id: number, data: UpdateCity): Observable<City> {
        return this.http.put<City>(`${this.baseUrl}/${id}`, data);
    }

    delete(id: number): Observable<void> {
        return this.http.delete<void>(`${this.baseUrl}/${id}`);
    }
}
