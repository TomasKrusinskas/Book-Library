import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormsModule } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription } from 'rxjs';

import { CartService } from '../../services/cart.service';
import { CartItem } from '../../models/order.models';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatInputModule,
    MatFormFieldModule,
    FormsModule
  ],
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.scss']
})
export class CartComponent implements OnInit, OnDestroy {
  cartItems: CartItem[] = [];
  total: number = 0;
  private subscription: Subscription = new Subscription();

  constructor(
    private cartService: CartService,
    private router: Router,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    this.subscription.add(
      this.cartService.getCartItems().subscribe(items => {
        this.cartItems = items;
        this.total = this.cartService.getCartTotal();
      })
    );
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  onQuantityChange(bookId: number, event: any): void {
    const value = (event.target as HTMLInputElement).value;
    const quantity = parseInt(value, 10);
    this.cartService.updateQuantity(bookId, quantity);
  }

  updateQuantity(bookId: number, quantity: number): void {
    this.cartService.updateQuantity(bookId, quantity);
  }

  removeItem(bookId: number): void {
    this.cartService.removeFromCart(bookId);
    this.snackBar.open('Item removed from cart', 'Close', { duration: 2000 });
  }

  clearCart(): void {
    this.cartService.clearCart();
    this.snackBar.open('Cart cleared', 'Close', { duration: 2000 });
  }

  proceedToCheckout(): void {
    if (this.cartItems.length === 0) {
      this.snackBar.open('Your cart is empty', 'Close', { duration: 2000 });
      return;
    }
    this.router.navigate(['/checkout']);
  }

  continueShopping(): void {
    this.router.navigate(['/books']);
  }

  getItemTotal(item: CartItem): number {
    return item.book.price * item.quantity;
  }

  onImageError(event: any): void {
    const target = event.target as HTMLImageElement;
    target.src = 'assets/images/book-placeholder.jpg';
  }
} 