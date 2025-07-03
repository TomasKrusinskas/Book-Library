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
    RouterModule
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

  constructor(
    private bookService: BookService,
    private genreService: GenreService,
    private route: ActivatedRoute
  ) {}

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

    // Load genres first
    this.genreService.getGenres().subscribe({
      next: (genres) => this.genres.set(genres),
      error: (error) => console.error('Error loading genres:', error)
    });

    // Load books based on selectedGenreId
    if (this.selectedGenreId === 0) {
      this.bookService.getBooks().subscribe({
        next: (books) => {
          this.books.set(books);
          this.filteredBooks.set(books);
          this.loading.set(false);
        },
        error: (error) => {
          console.error('Error loading books:', error);
          this.loading.set(false);
        }
      });
    } else {
      this.bookService.getBooksByGenre(this.selectedGenreId).subscribe({
        next: (books) => {
          this.books.set(books);
          this.filteredBooks.set(books);
          this.loading.set(false);
        },
        error: (error) => {
          console.error('Error filtering books:', error);
          this.loading.set(false);
        }
      });
    }
  }

  filterBooks(): void {
    this.currentPage = 1; // Reset to first page when filtering
    if (this.selectedGenreId === 0) {
      this.filteredBooks.set(this.books());
    } else {
      this.loading.set(true);
      this.bookService.getBooksByGenre(this.selectedGenreId).subscribe({
        next: (books) => {
          this.filteredBooks.set(books);
          this.loading.set(false);
        },
        error: (error) => {
          console.error('Error filtering books:', error);
          this.loading.set(false);
        }
      });
    }
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
}
