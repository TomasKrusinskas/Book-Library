import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { BookListComponent } from './components/book-list/book-list.component';
import { GenreListComponent } from './components/genre-list/genre-list.component';
import { AdminDashboardComponent } from './components/admin-dashboard/admin-dashboard.component';
import { MainPageComponent } from './components/main-page/main-page.component';
import { BookDetailsComponent } from './components/book-details/book-details.component';
import { FavoritesComponent } from './components/favorites/favorites.component';
import { ProfileComponent } from './components/profile/profile.component';
import { CartComponent } from './components/cart/cart.component';
import { CheckoutComponent } from './components/checkout/checkout.component';
import { OrdersComponent } from './components/orders/orders.component';
import { PaymentComponent } from './components/payment/payment.component';

export const routes: Routes = [
  { path: '', component: MainPageComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'books', component: BookListComponent },
  { path: 'genres', component: GenreListComponent },
  { path: 'books/:id', component: BookDetailsComponent },
  { path: 'favorites', component: FavoritesComponent },
  { path: 'profile', component: ProfileComponent },
  { path: 'cart', component: CartComponent },
  { path: 'checkout', component: CheckoutComponent },
  { path: 'orders', component: OrdersComponent },
  { path: 'payment/:orderId', component: PaymentComponent },
  {
    path: 'admin',
    component: AdminDashboardComponent,
    // Add auth guard here later
  }
];
