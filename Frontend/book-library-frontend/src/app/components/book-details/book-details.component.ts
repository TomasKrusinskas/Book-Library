import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {ActivatedRoute, RouterLink} from '@angular/router';
import { BookService } from '../../services/book.service';
import { Book } from '../../models/book.models';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { Location } from '@angular/common';
import {MatProgressSpinner} from '@angular/material/progress-spinner';

@Component({
  selector: 'app-book-details',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatButtonModule, MatProgressSpinner, RouterLink],
  templateUrl: './book-details.component.html',
  styleUrls: ['./book-details.component.scss']
})
export class BookDetailsComponent implements OnInit {
  book: Book | null = null;
  loading = true;
  notFound = false;

  constructor(private route: ActivatedRoute, private bookService: BookService, private location: Location) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (id) {
      this.bookService.getBook(id).subscribe({
        next: (book: Book) => {
          this.book = book;
          this.loading = false;
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
}
