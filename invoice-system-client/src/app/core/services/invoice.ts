import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { ApiResponse } from '../../models/api-response.models';
import { PagedResult } from '../../models/paged-result.models';
import {
  CreateInvoiceRequest,
  InvoiceListResponse,
  InvoiceResponse,
  UpdateInvoiceRequest
} from '../../models/invoice.models';

@Injectable({
  providedIn: 'root'
})
export class InvoiceService {
  private http = inject(HttpClient);
  private baseUrl = `${environment.apiUrl}/invoices`;

  getAll(
    pageNumber: number = 1,
    pageSize: number = 10,
    search: string = ''
  ): Observable<ApiResponse<PagedResult<InvoiceListResponse>>> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);

    if (search.trim()) {
      params = params.set('search', search.trim());
    }

    return this.http.get<ApiResponse<PagedResult<InvoiceListResponse>>>(
      this.baseUrl,
      { params }
    );
  }

  getById(id: number): Observable<ApiResponse<InvoiceResponse>> {
    return this.http.get<ApiResponse<InvoiceResponse>>(`${this.baseUrl}/${id}`);
  }

  create(request: CreateInvoiceRequest): Observable<ApiResponse<InvoiceResponse>> {
    return this.http.post<ApiResponse<InvoiceResponse>>(this.baseUrl, request);
  }

  update(id: number, request: UpdateInvoiceRequest): Observable<ApiResponse<InvoiceResponse>> {
    return this.http.put<ApiResponse<InvoiceResponse>>(`${this.baseUrl}/${id}`, request);
  }

  delete(id: number): Observable<ApiResponse<null>> {
    return this.http.delete<ApiResponse<null>>(`${this.baseUrl}/${id}`);
  }
}