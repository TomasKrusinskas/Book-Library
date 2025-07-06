import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap, switchMap } from 'rxjs';
import { LoginDto, RegisterDto, TokenResponse } from '../models/auth.models';
import { User } from '../models/user.models';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private tokenKey = 'auth_token';
  private userSignal = signal<User | null>(null);

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

  login(credentials: LoginDto): Observable<User> {
    return this.http.post<TokenResponse>(`${environment.apiUrl}/auth/login`, credentials)
      .pipe(
        tap(response => {
          console.log('Login response:', response);
          localStorage.setItem(this.tokenKey, response.token);
        }),
        switchMap(() => {
          console.log('Loading user profile after login...');
          return this.loadUserProfile();
        })
      );
  }

  register(data: RegisterDto): Observable<User> {
    return this.http.post<TokenResponse>(`${environment.apiUrl}/auth/register`, data)
      .pipe(
        tap(response => {
          console.log('Register response:', response);
          localStorage.setItem(this.tokenKey, response.token);
        }),
        switchMap(() => {
          console.log('Loading user profile after register...');
          return this.loadUserProfile();
        })
      );
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
    this.userSignal.set(null);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  loadUserProfile(): Observable<User> {
    console.log('Calling profile endpoint...');
    return this.http.get<User>(`${environment.apiUrl}/auth/profile`)
      .pipe(
        tap(user => {
          console.log('Profile loaded:', user);
          this.userSignal.set(user);
        })
      );
  }

  private loadStoredUser(): void {
    const token = this.getToken();
    if (token) {
      console.log('Token found, loading profile...');
      // Try to load the full user profile
      this.loadUserProfile().subscribe({
        next: (user) => {
          console.log('Profile loaded on startup:', user);
        },
        error: (error) => {
          console.log('Profile loading failed, falling back to token:', error);
          // If profile loading fails, fall back to token decoding
          try {
            const payload = JSON.parse(atob(token.split('.')[1]));
            const roles = payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
            const user: User = {
              id: payload.sub || '',
              email: payload.email || '',
              firstName: '',
              lastName: '',
              userName: payload.email || '',
              roles: Array.isArray(roles) ? roles : [roles],
              createdAt: new Date().toISOString(),
              isActive: true
            };
            console.log('Fallback user from token:', user);
            this.userSignal.set(user);
          } catch (error) {
            console.error('Token decoding failed:', error);
            this.logout();
          }
        }
      });
    }
  }
}
