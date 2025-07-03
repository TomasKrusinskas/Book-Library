import { Component, OnInit, signal, TemplateRef, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatTabsModule } from '@angular/material/tabs';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { BookService } from '../../services/book.service';
import { GenreService } from '../../services/genre.service';
import { Book, CreateBookDto } from '../../models/book.models';
import { Genre, CreateGenreDto } from '../../models/genre.models';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatTabsModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatSnackBarModule
  ],
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.scss']
})
export class AdminDashboardComponent implements OnInit {
  @ViewChild('bookDialog') bookDialogTemplate!: TemplateRef<any>;
  @ViewChild('genreDialog') genreDialogTemplate!: TemplateRef<any>;

  // Signals for data
  books = signal<Book[]>([]);
  genres = signal<Genre[]>([]);
  loadingBooks = signal(true);
  loadingGenres = signal(true);

  // Table columns
  bookColumns = ['id', 'title', 'author', 'genre', 'year', 'actions'];
  genreColumns = ['id', 'name', 'description', 'actions'];

  // Forms
  bookForm: FormGroup;
  genreForm: FormGroup;

  // Edit state
  editingBook: Book | null = null;
  editingGenre: Genre | null = null;

  constructor(
    private fb: FormBuilder,
    private bookService: BookService,
    private genreService: GenreService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar
  ) {
    this.bookForm = this.fb.group({
      title: ['', Validators.required],
      author: ['', Validators.required],
      isbn: ['', Validators.required],
      description: [''],
      publicationYear: ['', Validators.required],
      genreId: ['', Validators.required]
    });

    this.genreForm = this.fb.group({
      name: ['', Validators.required],
      description: ['']
    });
  }

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.loadBooks();
    this.loadGenres();
  }

  loadBooks(): void {
    this.loadingBooks.set(true);
    this.bookService.getBooks().subscribe({
      next: (books) => {
        this.books.set(books);
        this.loadingBooks.set(false);
      },
      error: (error) => {
        console.error('Error loading books:', error);
        this.snackBar.open('Failed to load books', 'Close', {duration: 3000});
        this.loadingBooks.set(false);
      }
    });
  }

  loadGenres(): void {
    this.loadingGenres.set(true);
    this.genreService.getGenres().subscribe({
      next: (genres) => {
        this.genres.set(genres);
        this.loadingGenres.set(false);
      },
      error: (error) => {
        console.error('Error loading genres:', error);
        this.snackBar.open('Failed to load genres', 'Close', {duration: 3000});
        this.loadingGenres.set(false);
      }
    });
  }

  // Book operations
  openBookDialog(book?: Book): void {
    this.editingBook = book || null;
    if (book) {
      this.bookForm.patchValue({
        title: book.title,
        author: book.author,
        isbn: book.isbn,
        description: book.description,
        publicationYear: book.publicationYear,
        genreId: book.genreId
      });
    } else {
      this.bookForm.reset();
    }
    this.dialog.open(this.bookDialogTemplate);
  }

  editBook(book: Book): void {
    this.openBookDialog(book);
  }

  saveBook(): void {
    if (this.bookForm.valid) {
      const bookData: CreateBookDto = this.bookForm.value;

      if (this.editingBook) {
        this.bookService.updateBook(this.editingBook.id, bookData).subscribe({
          next: () => {
            this.snackBar.open('Book updated successfully', 'Close', {duration: 3000});
            this.dialog.closeAll();
            this.loadBooks();
          },
          error: (error) => {
            this.snackBar.open('Failed to update book', 'Close', {duration: 3000});
          }
        });
      } else {
        this.bookService.createBook(bookData).subscribe({
          next: () => {
            this.snackBar.open('Book created successfully', 'Close', {duration: 3000});
            this.dialog.closeAll();
            this.loadBooks();
          },
          error: (error) => {
            this.snackBar.open('Failed to create book', 'Close', {duration: 3000});
          }
        });
      }
    }
  }

  deleteBook(book: Book): void {
    if (confirm(`Are you sure you want to delete "${book.title}"?`)) {
      this.bookService.deleteBook(book.id).subscribe({
        next: () => {
          this.snackBar.open('Book deleted successfully', 'Close', {duration: 3000});
          this.loadBooks();
        },
        error: (error) => {
          this.snackBar.open('Failed to delete book', 'Close', {duration: 3000});
        }
      });
    }
  }

  // Genre operations
  openGenreDialog(genre?: Genre): void {
    this.editingGenre = genre || null;
    if (genre) {
      this.genreForm.patchValue({
        name: genre.name,
        description: genre.description
      });
    } else {
      this.genreForm.reset();
    }
    this.dialog.open(this.genreDialogTemplate);
  }

  editGenre(genre: Genre): void {
    this.openGenreDialog(genre);
  }

  saveGenre(): void {
    if (this.genreForm.valid) {
      const genreData: CreateGenreDto = this.genreForm.value;

      if (this.editingGenre) {
        this.genreService.updateGenre(this.editingGenre.id, genreData).subscribe({
          next: () => {
            this.snackBar.open('Genre updated successfully', 'Close', {duration: 3000});
            this.dialog.closeAll();
            this.loadGenres();
          },
          error: (error) => {
            this.snackBar.open('Failed to update genre', 'Close', {duration: 3000});
          }
        });
      } else {
        this.genreService.createGenre(genreData).subscribe({
          next: () => {
            this.snackBar.open('Genre created successfully', 'Close', {duration: 3000});
            this.dialog.closeAll();
            this.loadGenres();
          },
          error: (error) => {
            this.snackBar.open('Failed to create genre', 'Close', {duration: 3000});
          }
        });
      }
    }
  }

  deleteGenre(genre: Genre): void {
    if (confirm(`Are you sure you want to delete "${genre.name}"?`)) {
      this.genreService.deleteGenre(genre.id).subscribe({
        next: () => {
          this.snackBar.open('Genre deleted successfully', 'Close', {duration: 3000});
          this.loadGenres();
        },
        error: (error) => {
          this.snackBar.open('Failed to delete genre', 'Close', {duration: 3000});
        }
      });
    }
  }
}
