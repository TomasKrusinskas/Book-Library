export interface Genre {
  id: number;
  name: string;
  description: string;
}

export interface CreateGenreDto {
  name: string;
  description: string;
}
