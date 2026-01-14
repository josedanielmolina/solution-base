import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '@environments/environment';
import { User, CreateUser, UpdateUser } from '@features/users/models/user.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private http = inject(HttpClient);
  private apiUrl = environment.apiUrl;
  private endpoint = '/api/users';

  getAll(): Observable<User[]> {
    return this.http.get<User[]>(`${this.apiUrl}${this.endpoint}`);
  }

  getById(id: number): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}${this.endpoint}/${id}`);
  }

  create(user: CreateUser): Observable<User> {
    return this.http.post<User>(`${this.apiUrl}${this.endpoint}`, user);
  }

  update(id: number, user: UpdateUser): Observable<User> {
    return this.http.put<User>(`${this.apiUrl}${this.endpoint}/${id}`, user);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}${this.endpoint}/${id}`);
  }
}
