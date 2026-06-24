import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

import { AuthService } from '../../core/services/auth';
import { RegisterRequest } from '../../models/auth.models';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './register.html',
  styleUrl: './register.scss'
})
export class RegisterComponent {
  private authService = inject(AuthService);
  private router = inject(Router);

  model: RegisterRequest = {
    fullName: '',
    email: '',
    password: ''
  };

  errorMessage = '';
  successMessage = '';
  isLoading = false;

  register(): void {
    this.errorMessage = '';
    this.successMessage = '';
    this.isLoading = true;

    this.authService.register(this.model).subscribe({
      next: () => {
        this.isLoading = false;
        this.successMessage = 'Account created successfully. Please login.';

        setTimeout(() => {
          this.router.navigate(['/login']);
        }, 1000);
      },
      error: (error: any) => {
        this.isLoading = false;
        this.errorMessage =
          error?.error?.message || 'Register failed. Please try again.';
      }
    });
  }
}