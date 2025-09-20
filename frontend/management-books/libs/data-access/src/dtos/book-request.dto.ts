export interface BookRequestDto {
  id?: number
  title?: string;
  isbn?: string;
  author?: string;
  synopsis?: string;
  genreId?: number;
  publisherId?: number;
  coverImagePath?: string;
}
