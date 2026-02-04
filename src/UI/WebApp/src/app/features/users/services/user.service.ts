import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '@environments/environment';
import { User, CreateUser, UpdateUser, UpdateProfile, AssignRoles } from '@features/users/models/user.model';

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

  updateProfile(profile: UpdateProfile): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}${this.endpoint}/profile`, profile);
  }

  assignRoles(userId: number, roles: AssignRoles): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}${this.endpoint}/${userId}/roles`, roles);
  }

  search(query: string, limit: number = 5): Observable<UserSearchResult[]> {
    return this.http.get<UserSearchResult[]>(`${this.apiUrl}${this.endpoint}/search`, {
      params: { q: query, limit: limit.toString() }
    });
  }
}

export interface UserSearchResult {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  document: string | null;
}

