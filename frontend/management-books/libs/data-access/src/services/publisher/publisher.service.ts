import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {
  ApiPaginatedFilterRequest,
  ApiResponseDto,
  BookChildDto,
} from '@management-books/data-access';
import { environment } from 'environments/environment';
import { Observable } from 'rxjs';

@Injectable()
export class PublisherService {
  constructor(private http: HttpClient) {}

  public getPaginatedPublisher(
    filter: ApiPaginatedFilterRequest,
  ): Observable<ApiResponseDto<BookChildDto>> {
    let params = new HttpParams();

    Object.entries(filter).forEach(([key, value]) => {
      if (value !== null && value !== undefined) {
        params = params.set(key, value as any);
      }
    });
    return this.http.get<ApiResponseDto<BookChildDto>>(`${environment.api_base_url}/publisher`, {
      params,
    });
  }

  public getPublisher(id: number): Observable<BookChildDto> {
    return this.http.get<BookChildDto>(`${environment.api_base_url}/publisher/${id}`);
  }

  public createPublisher(request: BookChildDto): Observable<BookChildDto> {
    return this.http.post<BookChildDto>(`${environment.api_base_url}/publisher`, {
      ...request,
    });
  }

  public updatePublisher(request: BookChildDto): Observable<BookChildDto> {
    return this.http.patch<BookChildDto>(`${environment.api_base_url}/publisher`, {
      ...request,
    });
  }

  public deletePublisher(id: number): Observable<BookChildDto> {
    return this.http.delete<BookChildDto>(`${environment.api_base_url}/publisher/${id}`);
  }
}
