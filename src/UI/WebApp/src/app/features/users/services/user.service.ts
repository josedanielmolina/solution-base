import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../../../core/services/api.service';
import { User, CreateUser, UpdateUser } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiService = inject(ApiService);
  private endpoint = '/api/users';

  getAll(): Observable<User[]> {
    return this.apiService.get<User[]>(this.endpoint);
  }

  getById(id: number): Observable<User> {
    return this.apiService.get<User>(`${this.endpoint}/${id}`);
  }

  create(user: CreateUser): Observable<User> {
    return this.apiService.post<User>(this.endpoint, user);
  }

  update(id: number, user: UpdateUser): Observable<User> {
    return this.apiService.put<User>(`${this.endpoint}/${id}`, user);
  }

  delete(id: number): Observable<void> {
    return this.apiService.delete<void>(`${this.endpoint}/${id}`);
  }
}
