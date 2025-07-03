export interface Book {
  id: number;
  title: string;
  author: string;
  isbn: string;
  description: string;
  publicationYear: number;
  genreId: number;
  genreName: string;
}

export interface CreateBookDto {
  title: string;
  author: string;
  isbn: string;
  description: string;
  publicationYear: number;
  genreId: number;
}
