import { Component, computed, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatBadgeModule } from '@angular/material/badge';
import { AuthService } from '../../services/auth.service';
import { CartService } from '../../services/cart.service';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatBadgeModule
  ],
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit, OnDestroy {
  cartItemCount = 0;
  private subscription: Subscription = new Subscription();

  constructor(
    public authService: AuthService,
    private cartService: CartService,
    private router: Router
  ) {}

  isLoggedIn = computed(() => this.authService.isLoggedIn);
  isAdmin = computed(() => this.authService.isAdmin);
  userEmail = computed(() => this.authService.user?.email || '');

  ngOnInit(): void {
    this.subscription.add(
      this.cartService.getCartItems().subscribe(items => {
        this.cartItemCount = this.cartService.getCartItemCount();
      })
    );
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
