import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';

import { OrderService } from '../../services/order.service';
import { OrderDto } from '../../models/order.models';

@Component({
  selector: 'app-payment',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatProgressSpinnerModule,
    ReactiveFormsModule
  ],
  templateUrl: './payment.component.html',
  styleUrls: ['./payment.component.scss']
})
export class PaymentComponent implements OnInit, OnDestroy {
  order: OrderDto | null = null;
  loading = true;
  processing = false;
  paymentForm: FormGroup;
  private subscription: Subscription = new Subscription();

  constructor(
    private orderService: OrderService,
    private router: Router,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private fb: FormBuilder
  ) {
    this.paymentForm = this.fb.group({
      cardNumber: ['', [Validators.required, Validators.pattern(/^\d{16}$/)]],
      expiryMonth: ['', [Validators.required, Validators.pattern(/^(0[1-9]|1[0-2])$/)]],
      expiryYear: ['', [Validators.required, Validators.pattern(/^\d{2}$/)]],
      cvv: ['', [Validators.required, Validators.pattern(/^\d{3,4}$/)]],
      cardholderName: ['', [Validators.required, Validators.minLength(2)]]
    });
  }

  ngOnInit(): void {
    const orderId = this.route.snapshot.params['orderId'];
    if (orderId) {
      this.loadOrder(orderId);
    } else {
      this.router.navigate(['/orders']);
    }
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  loadOrder(orderId: string): void {
    this.loading = true;
    this.subscription.add(
      this.orderService.getOrder(parseInt(orderId)).subscribe({
        next: (order) => {
          this.order = order;
          this.loading = false;
        },
        error: (error) => {
          console.error('Error loading order:', error);
          this.snackBar.open('Error loading order', 'Close', { duration: 3000 });
          this.router.navigate(['/orders']);
        }
      })
    );
  }

  formatCardNumber(event: any): void {
    let value = event.target.value.replace(/\D/g, '');
    value = value.replace(/(\d{4})(?=\d)/g, '$1 ');
    event.target.value = value.substring(0, 19);
  }

  formatExpiry(event: any): void {
    let value = event.target.value.replace(/\D/g, '');
    if (value.length >= 2) {
      value = value.substring(0, 2) + '/' + value.substring(2, 4);
    }
    event.target.value = value.substring(0, 5);
  }

  async processPayment(): Promise<void> {
    if (this.paymentForm.invalid || !this.order) {
      return;
    }

    this.processing = true;

    try {
      // Simulate Stripe payment processing
      await this.simulateStripePayment();
      
      // Update order status to confirmed
      this.subscription.add(
        this.orderService.updateOrderStatus(this.order.id, 'confirmed').subscribe({
          next: () => {
            this.snackBar.open('Payment successful! Your order has been confirmed.', 'Close', { duration: 5000 });
            this.router.navigate(['/orders']);
          },
          error: (error) => {
            console.error('Error updating order status:', error);
            this.snackBar.open('Payment successful but there was an issue updating your order. Please contact support.', 'Close', { duration: 5000 });
            this.router.navigate(['/orders']);
          }
        })
      );
    } catch (error) {
      this.snackBar.open('Payment failed. Please try again.', 'Close', { duration: 3000 });
      this.processing = false;
    }
  }

  private simulateStripePayment(): Promise<void> {
    return new Promise((resolve, reject) => {
      // Simulate API call delay
      setTimeout(() => {
        // Simulate 95% success rate
        if (Math.random() > 0.05) {
          resolve();
        } else {
          reject(new Error('Payment failed'));
        }
      }, 2000);
    });
  }

  cancelPayment(): void {
    this.router.navigate(['/orders']);
  }

  getCurrentYear(): number {
    return new Date().getFullYear();
  }

  onImageError(event: any): void {
    const target = event.target as HTMLImageElement;
    target.src = 'assets/images/book-placeholder.jpg';
  }
} 