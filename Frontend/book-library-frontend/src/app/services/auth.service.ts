import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { LoginDto, RegisterDto, TokenResponse } from '../models/auth.models';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private tokenKey = 'auth_token';
  private userSignal = signal<TokenResponse | null>(null);

  constructor(private http: HttpClient) {
    this.loadStoredUser();
  }

  get user() {
    return this.userSignal();
  }

  get isLoggedIn() {
    return !!this.userSignal();
  }

  get isAdmin() {
    return this.userSignal()?.roles.includes('Admin') ?? false;
  }

  login(credentials: LoginDto): Observable<TokenResponse> {
    return this.http.post<TokenResponse>(`${environment.apiUrl}/auth/login`, credentials)
      .pipe(
        tap(response => this.handleAuthResponse(response))
      );
  }

  register(data: RegisterDto): Observable<TokenResponse> {
    return this.http.post<TokenResponse>(`${environment.apiUrl}/auth/register`, data)
      .pipe(
        tap(response => this.handleAuthResponse(response))
      );
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
    this.userSignal.set(null);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  private handleAuthResponse(response: TokenResponse): void {
    localStorage.setItem(this.tokenKey, response.token);
    this.userSignal.set(response);
  }

  private loadStoredUser(): void {
    const token = this.getToken();
    if (token) {
      // Decode JWT token to get user info
      try {
        const payload = JSON.parse(atob(token.split('.')[1]));
        const roles = payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
        this.userSignal.set({
          token,
          email: payload.email,
          roles: Array.isArray(roles) ? roles : [roles]
        });
      } catch (error) {
        this.logout();
      }
    }
  }
}
