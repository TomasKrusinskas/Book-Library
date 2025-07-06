import { Book } from './book.models';

export interface UserFavorite {
  id: number;
  userId: string;
  bookId: number;
  createdAt: string;
  book: Book;
}

export interface CreateUserFavoriteDto {
  bookId: number;
} 