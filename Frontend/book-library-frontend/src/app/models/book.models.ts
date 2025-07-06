export interface Book {
  id: number;
  title: string;
  author: string;
  isbn: string;
  description: string;
  publicationYear: number;
  genreId: number;
  genreName: string;
  averageRating: number | null;
  summary: string;
  photoUrl: string;
  price: number;
  myRating?: number;
  isFavorite?: boolean;
}

export interface CreateBookDto {
  title: string;
  author: string;
  isbn: string;
  description: string;
  publicationYear: number;
  genreId: number;
  summary: string;
  photoUrl: string;
  price: number;
}
