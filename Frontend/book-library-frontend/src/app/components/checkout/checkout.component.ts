import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription } from 'rxjs';

import { CartService } from '../../services/cart.service';
import { OrderService } from '../../services/order.service';
import { CartItem } from '../../models/order.models';

@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatInputModule,
    MatFormFieldModule
  ],
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss']
})
export class CheckoutComponent implements OnInit, OnDestroy {
  checkoutForm: FormGroup;
  cartItems: CartItem[] = [];
  total: number = 0;
  isSubmitting = false;
  private subscription: Subscription = new Subscription();

  constructor(
    private formBuilder: FormBuilder,
    private cartService: CartService,
    private orderService: OrderService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {
    this.checkoutForm = this.formBuilder.group({
      shippingAddress: ['', [Validators.required, Validators.minLength(10)]],
      notes: ['']
    });
  }

  ngOnInit(): void {
    this.cartItems = this.cartService.getCartItemsValue();
    this.total = this.cartService.getCartTotal();

    if (this.cartItems.length === 0) {
      this.router.navigate(['/cart']);
      return;
    }
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  onSubmit(): void {
    if (this.checkoutForm.invalid) {
      this.snackBar.open('Please fill in all required fields', 'Close', { duration: 3000 });
      return;
    }

    this.isSubmitting = true;

    const orderData = {
      items: this.cartItems.map(item => ({
        bookId: item.book.id,
        quantity: item.quantity
      })),
      shippingAddress: this.checkoutForm.value.shippingAddress,
      notes: this.checkoutForm.value.notes
    };

    this.subscription.add(
      this.orderService.createOrder(orderData).subscribe({
        next: (response) => {
          this.cartService.clearCart();
          this.snackBar.open('Order placed successfully!', 'Close', { duration: 5000 });
          this.router.navigate(['/orders']);
        },
        error: (error) => {
          console.error('Error creating order:', error);
          this.snackBar.open('Error placing order. Please try again.', 'Close', { duration: 5000 });
          this.isSubmitting = false;
        }
      })
    );
  }

  goBack(): void {
    this.router.navigate(['/cart']);
  }

  getItemTotal(item: CartItem): number {
    return item.book.price * item.quantity;
  }

  getErrorMessage(fieldName: string): string {
    const field = this.checkoutForm.get(fieldName);
    if (field?.hasError('required')) {
      return `${fieldName.charAt(0).toUpperCase() + fieldName.slice(1)} is required`;
    }
    if (field?.hasError('minlength')) {
      return `${fieldName.charAt(0).toUpperCase() + fieldName.slice(1)} must be at least ${field.errors?.['minlength'].requiredLength} characters`;
    }
    return '';
  }

  onImageError(event: any): void {
    const target = event.target as HTMLImageElement;
    target.src = 'assets/images/book-placeholder.jpg';
  }
} 