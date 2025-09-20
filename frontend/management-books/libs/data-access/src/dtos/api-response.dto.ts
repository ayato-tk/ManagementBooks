export interface ApiResponseDto<T> {
  data: T[];
  pageSize: number;
  page: number;
  totalItems: number;
  totalPages: number;
}
