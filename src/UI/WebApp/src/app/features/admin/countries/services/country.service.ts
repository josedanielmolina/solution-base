import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '@environments/environment';
import { Country, CreateCountry, UpdateCountry } from '../models/country.model';

@Injectable({ providedIn: 'root' })
export class CountryService {
    private http = inject(HttpClient);
    private baseUrl = `${environment.apiUrl}/api/countries`;

    getAll(activeOnly = true): Observable<Country[]> {
        return this.http.get<Country[]>(`${this.baseUrl}?activeOnly=${activeOnly}`);
    }

    getById(id: number): Observable<Country> {
        return this.http.get<Country>(`${this.baseUrl}/${id}`);
    }

    create(data: CreateCountry): Observable<Country> {
        return this.http.post<Country>(this.baseUrl, data);
    }

    update(id: number, data: UpdateCountry): Observable<Country> {
        return this.http.put<Country>(`${this.baseUrl}/${id}`, data);
    }

    delete(id: number): Observable<void> {
        return this.http.delete<void>(`${this.baseUrl}/${id}`);
    }
}
