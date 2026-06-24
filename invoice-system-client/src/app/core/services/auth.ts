import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';

import { environment } from '../../../environments/environment';
import { ApiResponse } from '../../models/api-response.models';
import {
  LoginRequest,
  LoginResponse,
  RegisterRequest,
  RegisterResponse
} from '../../models/auth.models';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private router = inject(Router);

  private tokenKey = 'invoice_token';
  private userKey = 'invoice_user';

  register(request: RegisterRequest): Observable<ApiResponse<RegisterResponse>> {
    return this.http.post<ApiResponse<RegisterResponse>>(
      `${environment.apiUrl}/auth/register`,
      request
    );
  }

  login(request: LoginRequest): Observable<ApiResponse<LoginResponse>> {
    return this.http
      .post<ApiResponse<LoginResponse>>(
        `${environment.apiUrl}/auth/login`,
        request
      )
      .pipe(
        tap(response => {
          if (response.success && response.data?.token) {
            this.saveLoginData(response.data);
          }
        })
      );
  }

  private saveLoginData(data: LoginResponse): void {
    localStorage.setItem(this.tokenKey, data.token);

    localStorage.setItem(
      this.userKey,
      JSON.stringify({
        email: data.email,
        fullName: data.fullName
      })
    );
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.userKey);
    this.router.navigateByUrl('/login');
  }

  getUser(): { email: string; fullName: string } | null {
    const user = localStorage.getItem(this.userKey);
    return user ? JSON.parse(user) : null;
  }
}