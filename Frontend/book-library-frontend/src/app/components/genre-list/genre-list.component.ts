import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { GenreService } from '../../services/genre.service';
import { Genre } from '../../models/genre.models';
import { FormsModule } from '@angular/forms';
import { MatSelectModule } from '@angular/material/select';

@Component({
  selector: 'app-genre-list',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    FormsModule,
    MatSelectModule
  ],
  templateUrl: './genre-list.component.html',
  styleUrls: ['./genre-list.component.scss']
})
export class GenreListComponent implements OnInit {
  genres = signal<Genre[]>([]);
  loading = signal(true);
  pageSizeOptions = [5, 10, 20];
  pageSize = 5;
  currentPage = 1;

  constructor(
    private genreService: GenreService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadGenres();
  }

  loadGenres(): void {
    this.loading.set(true);
    this.genreService.getGenres().subscribe({
      next: (genres) => {
        this.genres.set(genres);
        this.loading.set(false);
      },
      error: (error) => {
        console.error('Error loading genres:', error);
        this.loading.set(false);
      }
    });
  }

  viewBooks(genreId: number): void {
    this.router.navigate(['/books'], { queryParams: { genre: genreId } });
  }

  get pagedGenres() {
    const start = (this.currentPage - 1) * this.pageSize;
    return this.genres().slice(start, start + this.pageSize);
  }

  get totalPages() {
    return Math.ceil(this.genres().length / this.pageSize) || 1;
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
