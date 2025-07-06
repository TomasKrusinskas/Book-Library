import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {ActivatedRoute, RouterLink} from '@angular/router';
import { BookService } from '../../services/book.service';
import { Book } from '../../models/book.models';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { Location } from '@angular/common';
import {MatProgressSpinner} from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { AuthService } from '../../services/auth.service';
import { UserFavoriteService } from '../../services/user-favorite.service';

@Component({
  selector: 'app-book-details',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatButtonModule, MatProgressSpinner, MatIconModule, MatSnackBarModule, RouterLink],
  templateUrl: './book-details.component.html',
  styleUrls: ['./book-details.component.scss']
})
export class BookDetailsComponent implements OnInit {
  book: Book | null = null;
  loading = true;
  notFound = false;
  myRating: number | null = null;
  ratingSubmitting = false;
  isLoggedIn = false;
  hoveredRating: number | null = null;

  constructor(
    private route: ActivatedRoute, 
    private bookService: BookService, 
    private location: Location, 
    private auth: AuthService,
    private userFavoriteService: UserFavoriteService,
    private snackBar: MatSnackBar
  ) {
    this.isLoggedIn = this.auth.isLoggedIn;
  }

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (id) {
      this.bookService.getBook(id).subscribe({
        next: (book: Book) => {
          this.book = book;
          this.loading = false;
          if (this.isLoggedIn) {
            this.bookService.getMyRating(id).subscribe(r => this.myRating = r ?? null);
            this.checkFavoriteStatus(book);
          }
        },
        error: () => {
          this.notFound = true;
          this.loading = false;
        }
      });
    } else {
      this.notFound = true;
      this.loading = false;
    }
  }

  goBack() {
    this.location.back();
  }

  setRating(rating: number) {
    if (!this.book) { this.ratingSubmitting = false; return; }
    this.ratingSubmitting = true;
    const bookId = this.book.id;
    this.bookService.rateBook(bookId, rating).subscribe({
      next: () => {
        this.myRating = rating;
        this.bookService.getBook(bookId).subscribe(b => this.book = b);
        this.ratingSubmitting = false;
      },
      error: () => {
        this.ratingSubmitting = false;
      }
    });
  }

  getStarsArray(rating: number | null): any[] {
    if (!rating) return [];
    return Array(Math.round(rating)).fill(0);
  }

  getEmptyStarsArray(rating: number | null): any[] {
    if (!rating) return Array(5).fill(0);
    return Array(5 - Math.round(rating)).fill(0);
  }

  setHover(star: number) {
    if (!this.ratingSubmitting) this.hoveredRating = star;
  }

  clearHover() {
    this.hoveredRating = null;
  }

  isStarFilled(star: number): boolean {
    return (this.hoveredRating !== null ? star <= this.hoveredRating : this.myRating !== null && star <= this.myRating);
  }

  // Favorite functionality
  checkFavoriteStatus(book: Book): void {
    this.userFavoriteService.isFavorite(book.id).subscribe({
      next: (isFavorite) => {
        book.isFavorite = isFavorite;
      },
      error: (error) => {
        console.error('Error checking favorite status:', error);
      }
    });
  }

  toggleFavorite(): void {
    if (!this.book || !this.isLoggedIn) {
      this.snackBar.open('Please log in to manage favorites', 'Close', { duration: 3000 });
      return;
    }

    if (this.book.isFavorite) {
      this.removeFromFavorites();
    } else {
      this.addToFavorites();
    }
  }

  addToFavorites(): void {
    if (!this.book) return;
    
    this.userFavoriteService.addToFavorites(this.book.id).subscribe({
      next: () => {
        this.book!.isFavorite = true;
        this.snackBar.open(`"${this.book!.title}" added to favorites`, 'Close', { duration: 3000 });
      },
      error: (error) => {
        console.error('Error adding to favorites:', error);
        this.snackBar.open('Failed to add to favorites', 'Close', { duration: 3000 });
      }
    });
  }

  removeFromFavorites(): void {
    if (!this.book) return;
    
    this.userFavoriteService.removeFromFavorites(this.book.id).subscribe({
      next: () => {
        this.book!.isFavorite = false;
        this.snackBar.open(`"${this.book!.title}" removed from favorites`, 'Close', { duration: 3000 });
      },
      error: (error) => {
        console.error('Error removing from favorites:', error);
        this.snackBar.open('Failed to remove from favorites', 'Close', { duration: 3000 });
      }
    });
  }
}
