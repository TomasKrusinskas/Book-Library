import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { CartItem } from '../models/order.models';
import { Book } from '../models/book.models';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private cartItems = new BehaviorSubject<CartItem[]>([]);

  constructor() {
    this.loadCartFromStorage();
  }

  getCartItems(): Observable<CartItem[]> {
    return this.cartItems.asObservable();
  }

  getCartItemsValue(): CartItem[] {
    return this.cartItems.value;
  }

  addToCart(book: Book, quantity: number = 1): void {
    const currentItems = this.cartItems.value;
    const existingItem = currentItems.find(item => item.book.id === book.id);

    if (existingItem) {
      existingItem.quantity += quantity;
      this.updateCart([...currentItems]);
    } else {
      this.updateCart([...currentItems, { book, quantity }]);
    }
  }

  removeFromCart(bookId: number): void {
    const currentItems = this.cartItems.value;
    const updatedItems = currentItems.filter(item => item.book.id !== bookId);
    this.updateCart(updatedItems);
  }

  updateQuantity(bookId: number, quantity: number): void {
    if (quantity <= 0) {
      this.removeFromCart(bookId);
      return;
    }

    const currentItems = this.cartItems.value;
    const updatedItems = currentItems.map(item => 
      item.book.id === bookId ? { ...item, quantity } : item
    );
    this.updateCart(updatedItems);
  }

  clearCart(): void {
    this.updateCart([]);
  }

  getCartTotal(): number {
    return this.cartItems.value.reduce((total, item) => 
      total + (item.book.price * item.quantity), 0
    );
  }

  getCartItemCount(): number {
    return this.cartItems.value.reduce((count, item) => count + item.quantity, 0);
  }

  private updateCart(items: CartItem[]): void {
    this.cartItems.next(items);
    this.saveCartToStorage();
  }

  private saveCartToStorage(): void {
    localStorage.setItem('cart', JSON.stringify(this.cartItems.value));
  }

  private loadCartFromStorage(): void {
    const savedCart = localStorage.getItem('cart');
    if (savedCart) {
      try {
        const items = JSON.parse(savedCart);
        this.cartItems.next(items);
      } catch (error) {
        console.error('Error loading cart from storage:', error);
        this.cartItems.next([]);
      }
    }
  }
} 