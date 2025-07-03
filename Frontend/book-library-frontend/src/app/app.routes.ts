import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { BookListComponent } from './components/book-list/book-list.component';
import { GenreListComponent } from './components/genre-list/genre-list.component';
import { AdminDashboardComponent } from './components/admin-dashboard/admin-dashboard.component';
import { MainPageComponent } from './components/main-page/main-page.component';
import { BookDetailsComponent } from './components/book-details/book-details.component';

export const routes: Routes = [
  { path: '', component: MainPageComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'books', component: BookListComponent },
  { path: 'genres', component: GenreListComponent },
  { path: 'books/:id', component: BookDetailsComponent },
  {
    path: 'admin',
    component: AdminDashboardComponent,
    // Add auth guard here later
  }
];
