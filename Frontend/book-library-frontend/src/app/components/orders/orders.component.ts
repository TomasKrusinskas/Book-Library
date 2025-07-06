import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription } from 'rxjs';

import { OrderService } from '../../services/order.service';
import { OrderDto } from '../../models/order.models';

@Component({
  selector: 'app-orders',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss']
})
export class OrdersComponent implements OnInit, OnDestroy {
  orders: OrderDto[] = [];
  loading = true;
  private subscription: Subscription = new Subscription();

  constructor(
    private orderService: OrderService,
    private router: Router,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    this.loadOrders();
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  loadOrders(): void {
    this.loading = true;
    this.subscription.add(
      this.orderService.getMyOrders().subscribe({
        next: (orders) => {
          this.orders = orders;
          this.loading = false;
        },
        error: (error) => {
          console.error('Error loading orders:', error);
          this.snackBar.open('Error loading orders', 'Close', { duration: 3000 });
          this.loading = false;
        }
      })
    );
  }

  getStatusColor(status: string): string {
    switch (status.toLowerCase()) {
      case 'pending':
        return 'warn';
      case 'confirmed':
        return 'primary';
      case 'shipped':
        return 'accent';
      case 'delivered':
        return 'primary';
      case 'cancelled':
        return 'warn';
      default:
        return 'primary';
    }
  }

  getStatusIcon(status: string): string {
    switch (status.toLowerCase()) {
      case 'pending':
        return 'schedule';
      case 'confirmed':
        return 'check_circle';
      case 'shipped':
        return 'local_shipping';
      case 'delivered':
        return 'done_all';
      case 'cancelled':
        return 'cancel';
      default:
        return 'help';
    }
  }

  formatDate(dateString: string): string {
    return new Date(dateString).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'long',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  }

  onImageError(event: any): void {
    const target = event.target as HTMLImageElement;
    target.src = 'assets/images/book-placeholder.jpg';
  }

  continueShopping(): void {
    this.router.navigate(['/books']);
  }

  payOrder(orderId: number): void {
    this.router.navigate(['/payment', orderId]);
  }
} 