import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { User } from '../models/user.models';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = `${environment.apiUrl}/auth`;

  constructor(private http: HttpClient) { }

  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(`${this.apiUrl}/users`);
  }

  updateUserRole(userId: string, role: string): Observable<any> {
    return this.http.put(`${this.apiUrl}/users/${userId}/role`, { role });
  }

  deactivateUser(userId: string): Observable<any> {
    return this.http.put(`${this.apiUrl}/users/${userId}/deactivate`, {});
  }

  activateUser(userId: string): Observable<any> {
    return this.http.put(`${this.apiUrl}/users/${userId}/activate`, {});
  }

  deleteUser(userId: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/users/${userId}`);
  }
} 