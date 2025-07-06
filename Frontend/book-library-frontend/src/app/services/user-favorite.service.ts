import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { UserFavorite, CreateUserFavoriteDto } from '../models/user-favorite.models';

@Injectable({
  providedIn: 'root'
})
export class UserFavoriteService {
  private apiUrl = `${environment.apiUrl}/userfavorites`;

  constructor(private http: HttpClient) { }

  getUserFavorites(): Observable<UserFavorite[]> {
    return this.http.get<UserFavorite[]>(this.apiUrl);
  }

  addToFavorites(bookId: number): Observable<UserFavorite> {
    const createFavoriteDto: CreateUserFavoriteDto = { bookId };
    return this.http.post<UserFavorite>(this.apiUrl, createFavoriteDto);
  }

  removeFromFavorites(bookId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${bookId}`);
  }

  isFavorite(bookId: number): Observable<boolean> {
    return this.http.get<boolean>(`${this.apiUrl}/check/${bookId}`);
  }
} 