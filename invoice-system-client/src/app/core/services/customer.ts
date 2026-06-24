import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { ApiResponse } from '../../models/api-response.models';
import { CustomerResponse } from '../../models/lookup.models';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {
  private http = inject(HttpClient);
  private baseUrl = `${environment.apiUrl}/customers`;

  getAll(): Observable<ApiResponse<CustomerResponse[]>> {
    return this.http.get<ApiResponse<CustomerResponse[]>>(this.baseUrl);
  }
}