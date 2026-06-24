import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

import { AuthService } from '../../core/services/auth';
import { LoginRequest } from '../../models/auth.models';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class LoginComponent {
  private authService = inject(AuthService);
  private router = inject(Router);

  model: LoginRequest = {
    email: '',
    password: ''
  };

  errorMessage = '';
  isLoading = false;

  login(): void {
    this.errorMessage = '';
    this.isLoading = true;

    this.authService.login(this.model).subscribe({
      next: (response) => {
        console.log('LOGIN SUCCESS:', response);

        this.isLoading = false;

        if (response.success && response.data?.token) {
          localStorage.setItem('invoice_token', response.data.token);

          localStorage.setItem(
            'invoice_user',
            JSON.stringify({
              email: response.data.email,
              fullName: response.data.fullName
            })
          );

          console.log('TOKEN SAVED:', localStorage.getItem('invoice_token'));

          this.router.navigateByUrl('/invoices').then(result => {
            console.log('NAVIGATION RESULT:', result);
          });

          return;
        }

        this.errorMessage = response.message || 'Login failed.';
      },
      error: (error: any) => {
        console.log('LOGIN ERROR:', error);

        this.isLoading = false;

        this.errorMessage =
          error?.error?.message ||
          error?.message ||
          'Login failed. Please try again.';
      }
    });
  }
}