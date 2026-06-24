import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { ApiResponse } from '../../models/api-response.models';
import { StoreResponse } from '../../models/lookup.models';

@Injectable({
  providedIn: 'root'
})
export class StoreService {
  private http = inject(HttpClient);
  private baseUrl = `${environment.apiUrl}/stores`;

  getAll(): Observable<ApiResponse<StoreResponse[]>> {
    return this.http.get<ApiResponse<StoreResponse[]>>(this.baseUrl);
  }
}