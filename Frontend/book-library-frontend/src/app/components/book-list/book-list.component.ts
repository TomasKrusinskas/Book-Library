// Updated book-list.component.ts
import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { ActivatedRoute } from '@angular/router';
import { BookService } from '../../services/book.service';
import { GenreService } from '../../services/genre.service';

import { Book } from '../../models/book.models';
import { Genre } from '../../models/genre.models';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';


@Component({
  selector: 'app-book-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatCardModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    MatSelectModule,
    RouterModule,
    MatSnackBarModule
  ],
  templateUrl: './book-list.component.html',
  styleUrls: ['./book-list.component.scss']
})
export class BookListComponent implements OnInit {
  books = signal<Book[]>([]);
  genres = signal<Genre[]>([]);
  filteredBooks = signal<Book[]>([]);
  loading = signal(true);
  selectedGenreId = 0;
  pageSizeOptions = [5, 10, 20];
  pageSize = 5;
  currentPage = 1;
  showRatedOnly = false;
  sortOption: 'none' | 'highest' | 'lowest' | 'yearAsc' | 'yearDesc' = 'none';
  isLoggedIn = false;

  constructor(
    private bookService: BookService,
    private genreService: GenreService,
    private route: ActivatedRoute,
    private auth: AuthService,
    private snackBar: MatSnackBar
  ) {
    this.isLoggedIn = this.auth.isLoggedIn;
  }

  ngOnInit(): void {
    // Check if a genre filter was passed in the URL
    this.route.queryParams.subscribe(params => {
      if (params['genre']) {
        this.selectedGenreId = Number(params['genre']);
      }
      this.loadData();
    });
  }

  loadData(): void {
    this.loading.set(true);
    this.genreService.getGenres().subscribe({
      next: (genres) => this.genres.set(genres),
      error: (error) => console.error('Error loading genres:', error)
    });
    this.bookService.getBooks().subscribe({
      next: (books) => {
        this.books.set(books);
        this.loading.set(false);
        this.filterBooks();
      },
      error: (error) => {
        console.error('Error loading books:', error);
        this.loading.set(false);
      }
    });
  }

  filterBooks(): void {
    this.currentPage = 1;
    let books = this.books();
    // Genre filter
    if (this.selectedGenreId && this.selectedGenreId !== 0) {
      const genreIdNum = Number(this.selectedGenreId);
      books = books.filter(b => {
        const bookGenreId = Number(b.genreId);
        const match = bookGenreId === genreIdNum;
        if (!match) {
          // Debug log for mismatches
          console.debug('Genre filter mismatch:', {bookTitle: b.title, bookGenreId, selectedGenreId: genreIdNum});
        }
        return match;
      });
    }
    // Rated filter
    if (this.showRatedOnly) {
      books = books.filter(b => b.averageRating && b.averageRating > 0);
    }
    // Sorting
    switch (this.sortOption) {
      case 'highest':
        books = books.slice().sort((a, b) => (b.averageRating ?? 0) - (a.averageRating ?? 0));
        break;
      case 'lowest':
        books = books.slice().sort((a, b) => (a.averageRating ?? 0) - (b.averageRating ?? 0));
        break;
      case 'yearAsc':
        books = books.slice().sort((a, b) => a.publicationYear - b.publicationYear);
        break;
      case 'yearDesc':
        books = books.slice().sort((a, b) => b.publicationYear - a.publicationYear);
        break;
    }
    this.filteredBooks.set(books);
  }

  get pagedBooks() {
    const start = (this.currentPage - 1) * this.pageSize;
    return this.filteredBooks().slice(start, start + this.pageSize);
  }

  get totalPages() {
    return Math.ceil(this.filteredBooks().length / this.pageSize) || 1;
  }

  setPage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
    }
  }

  setPageSize(size: any) {
    if (typeof size === 'object' && size.target) {
      this.pageSize = Number(size.target.value);
    } else {
      this.pageSize = Number(size);
    }
    this.currentPage = 1;
  }

  getStarsArray(rating: number | null): any[] {
    if (!rating) return [];
    return Array(Math.round(rating)).fill(0);
  }
  getEmptyStarsArray(rating: number | null): any[] {
    if (!rating) return Array(5).fill(0);
    return Array(5 - Math.round(rating)).fill(0);
  }


}
