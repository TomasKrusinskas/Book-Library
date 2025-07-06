import { Book } from './book.models';

export interface CreateOrderDto {
  items: OrderItemDto[];
  shippingAddress: string;
  notes?: string;
}

export interface OrderItemDto {
  bookId: number;
  quantity: number;
}

export interface OrderDto {
  id: number;
  userOrderNumber: number; // User-specific order number
  userId: string;
  userEmail: string;
  orderDate: string;
  totalAmount: number;
  status: string;
  shippingAddress: string;
  notes?: string;
  items: OrderItemDetailDto[];
}

export interface OrderItemDetailDto {
  id: number;
  bookId: number;
  bookTitle: string;
  bookAuthor: string;
  bookPhotoUrl: string;
  quantity: number;
  unitPrice: number;
  totalPrice: number;
}

export interface UpdateOrderStatusDto {
  status: string;
}

export interface CartItem {
  book: Book;
  quantity: number;
} 