import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';

export const authGuard: CanActivateFn = () => {
  const router = inject(Router);
  const token = localStorage.getItem('invoice_token');

  console.log('GUARD TOKEN:', token);

  if (token && token.length > 0) {
    return true;
  }

  return router.createUrlTree(['/login']);
};