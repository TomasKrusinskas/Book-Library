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
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatMenuModule } from '@angular/material/menu';
import { BookService } from '../../services/book.service';
import { GenreService } from '../../services/genre.service';
import { UserService } from '../../services/user.service';
import { Book, CreateBookDto } from '../../models/book.models';
import { Genre, CreateGenreDto } from '../../models/genre.models';
import { User } from '../../models/user.models';

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
    MatSnackBarModule,
    MatCardModule,
    MatChipsModule,
    MatMenuModule
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
  users = signal<User[]>([]);
  loadingBooks = signal(true);
  loadingGenres = signal(true);
  loadingUsers = signal(true);

  // Table columns
  bookColumns = ['id', 'title', 'author', 'genre', 'year', 'actions'];
  genreColumns = ['id', 'name', 'description', 'actions'];
  userColumns = ['id', 'name', 'email', 'roles', 'createdAt', 'actions'];

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
    private userService: UserService,
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
    this.loadUsers();
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

  loadUsers(): void {
    this.loadingUsers.set(true);
    this.userService.getUsers().subscribe({
      next: (users) => {
        this.users.set(users);
        this.loadingUsers.set(false);
      },
      error: (error) => {
        console.error('Error loading users:', error);
        this.snackBar.open('Failed to load users', 'Close', {duration: 3000});
        this.loadingUsers.set(false);
      }
    });
  }

  getFullName(user: User): string {
    return `${user.firstName} ${user.lastName}`.trim() || user.userName;
  }

  formatDate(dateString: string): string {
    return new Date(dateString).toLocaleDateString();
  }

  // Computed properties for user statistics
  get totalUsers(): number {
    return this.users().length;
  }

  get adminUsers(): number {
    return this.users().filter(u => u.roles.includes('Admin')).length;
  }

  get regularUsers(): number {
    return this.users().filter(u => u.roles.includes('User')).length;
  }

  // User management methods
  changeUserRole(user: User, newRole: string): void {
    if (confirm(`Are you sure you want to change ${this.getFullName(user)}'s role to ${newRole}?`)) {
      this.userService.updateUserRole(user.id, newRole).subscribe({
        next: () => {
          this.snackBar.open('User role updated successfully', 'Close', {duration: 3000});
          this.loadUsers();
        },
        error: (error) => {
          this.snackBar.open('Failed to update user role', 'Close', {duration: 3000});
        }
      });
    }
  }

  toggleUserStatus(user: User): void {
    const action = user.isActive ? 'deactivate' : 'activate';
    const userName = this.getFullName(user);
    
    if (confirm(`Are you sure you want to ${action} ${userName}?`)) {
      const serviceCall = user.isActive 
        ? this.userService.deactivateUser(user.id)
        : this.userService.activateUser(user.id);

      serviceCall.subscribe({
        next: () => {
          this.snackBar.open(`User ${action}d successfully`, 'Close', {duration: 3000});
          this.loadUsers();
        },
        error: (error) => {
          this.snackBar.open(`Failed to ${action} user`, 'Close', {duration: 3000});
        }
      });
    }
  }

  deleteUser(user: User): void {
    const userName = this.getFullName(user);
    if (confirm(`Are you sure you want to delete ${userName}? This action cannot be undone.`)) {
      this.userService.deleteUser(user.id).subscribe({
        next: () => {
          this.snackBar.open('User deleted successfully', 'Close', {duration: 3000});
          this.loadUsers();
        },
        error: (error) => {
          this.snackBar.open('Failed to delete user', 'Close', {duration: 3000});
        }
      });
    }
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
