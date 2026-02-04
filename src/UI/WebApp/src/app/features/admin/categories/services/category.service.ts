import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '@environments/environment';
import { Category, CreateCategory, UpdateCategory } from '../models/category.model';

@Injectable({ providedIn: 'root' })
export class CategoryService {
    private http = inject(HttpClient);
    private baseUrl = `${environment.apiUrl}/api/categories`;

    getAll(activeOnly = true): Observable<Category[]> {
        return this.http.get<Category[]>(`${this.baseUrl}?activeOnly=${activeOnly}`);
    }

    getByCountry(countryId: number, gender?: number): Observable<Category[]> {
        let url = `${this.baseUrl}/by-country/${countryId}`;
        if (gender) url += `?gender=${gender}`;
        return this.http.get<Category[]>(url);
    }

    getById(id: number): Observable<Category> {
        return this.http.get<Category>(`${this.baseUrl}/${id}`);
    }

    create(data: CreateCategory): Observable<Category> {
        return this.http.post<Category>(this.baseUrl, data);
    }

    update(id: number, data: UpdateCategory): Observable<Category> {
        return this.http.put<Category>(`${this.baseUrl}/${id}`, data);
    }

    delete(id: number): Observable<void> {
        return this.http.delete<void>(`${this.baseUrl}/${id}`);
    }
}
