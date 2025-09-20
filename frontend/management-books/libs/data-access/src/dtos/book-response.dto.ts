export interface BookResponseDto {
  id: number;
  title: string;
  isbn: string;
  author: string;
  synopsis: string;
  coverImagePath: string | null;
  publisherId: number;
  genreId: number;
}
