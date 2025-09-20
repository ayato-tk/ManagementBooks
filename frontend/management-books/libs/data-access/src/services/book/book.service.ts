import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {
  ApiPaginatedFilterRequest,
  ApiResponseDto,
  BookRequestDto,
  BookResponseDto,
} from '../../dtos';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment';

@Injectable()
export class BookService {
  constructor(private http: HttpClient) {}

  public getPaginatedBooks(
    filter: ApiPaginatedFilterRequest,
  ): Observable<ApiResponseDto<BookResponseDto>> {
    let params = new HttpParams();

    Object.entries(filter).forEach(([key, value]) => {
      if (value !== null && value !== undefined) {
        params = params.set(key, value as any);
      }
    });
    return this.http.get<ApiResponseDto<BookResponseDto>>(`${environment.api_base_url}/book`, {
      params,
    });
  }

  public getBook(id: number): Observable<BookResponseDto> {
    return this.http.get<BookResponseDto>(`${environment.api_base_url}/book/${id}`);
  }

  public getBookReport(): Observable<Blob> {
    return this.http.get(`${environment.api_base_url}/book/report`, { responseType: 'blob' });
  }

  public createBook(request: BookRequestDto): Observable<BookResponseDto> {
    return this.http.post<BookResponseDto>(`${environment.api_base_url}/book`, {
      ...request,
    });
  }

  public updateBook(request: BookRequestDto): Observable<BookResponseDto> {
    return this.http.patch<BookResponseDto>(`${environment.api_base_url}/book`, {
      ...request,
    });
  }

  public deleteBook(id: number): Observable<BookResponseDto> {
    return this.http.delete<BookResponseDto>(`${environment.api_base_url}/book/${id}`);
  }
}
