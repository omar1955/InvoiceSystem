import { Routes } from '@angular/router';

import { LoginComponent } from './auth/login/login';
import { RegisterComponent } from './auth/register/register';
import { InvoiceListComponent } from './invoices/invoice-list/invoice-list';
import { InvoiceFormComponent } from './invoices/invoice-form/invoice-form';
import { authGuard } from './core/guards/auth-guard';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },

  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },

  {
    path: 'invoices',
    component: InvoiceListComponent,
    canActivate: [authGuard]
  },
  {
    path: 'invoices/create',
    component: InvoiceFormComponent,
    canActivate: [authGuard]
  },
  {
    path: 'invoices/edit/:id',
    component: InvoiceFormComponent,
    canActivate: [authGuard]
  },

  { path: '**', redirectTo: 'login' }
];