import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { RouterModule } from '@angular/router';
import { UserFavoriteService } from '../../services/user-favorite.service';
import { UserFavorite } from '../../models/user-favorite.models';

@Component({
  selector: 'app-favorites',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
    RouterModule
  ],
  templateUrl: './favorites.component.html',
  styleUrls: ['./favorites.component.scss']
})
export class FavoritesComponent implements OnInit {
  favorites = signal<UserFavorite[]>([]);
  loading = signal(true);

  constructor(
    private userFavoriteService: UserFavoriteService,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    this.loadFavorites();
  }

  loadFavorites(): void {
    this.loading.set(true);
    this.userFavoriteService.getUserFavorites().subscribe({
      next: (favorites) => {
        this.favorites.set(favorites);
        this.loading.set(false);
      },
      error: (error) => {
        console.error('Error loading favorites:', error);
        this.snackBar.open('Failed to load favorites', 'Close', { duration: 3000 });
        this.loading.set(false);
      }
    });
  }

  removeFromFavorites(bookId: number, bookTitle: string): void {
    if (confirm(`Are you sure you want to remove "${bookTitle}" from your favorites?`)) {
      this.userFavoriteService.removeFromFavorites(bookId).subscribe({
        next: () => {
          this.snackBar.open('Book removed from favorites', 'Close', { duration: 3000 });
          this.loadFavorites(); // Reload the list
        },
        error: (error) => {
          console.error('Error removing from favorites:', error);
          this.snackBar.open('Failed to remove from favorites', 'Close', { duration: 3000 });
        }
      });
    }
  }

  getStarsArray(rating: number | null): any[] {
    if (!rating) return [];
    return Array(Math.round(rating)).fill(0);
  }

  getEmptyStarsArray(rating: number | null): any[] {
    if (!rating) return Array(5).fill(0);
    return Array(5 - Math.round(rating)).fill(0);
  }

  formatDate(dateString: string): string {
    return new Date(dateString).toLocaleDateString();
  }

  onImageError(event: any): void {
    // Hide the broken image and show placeholder
    event.target.style.display = 'none';
    const placeholder = event.target.parentElement.querySelector('.cover-placeholder');
    if (placeholder) {
      placeholder.style.display = 'flex';
    }
  }
} 