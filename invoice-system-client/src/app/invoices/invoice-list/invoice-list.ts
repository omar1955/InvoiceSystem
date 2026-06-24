import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, inject, NgZone, OnDestroy, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { finalize, timeout } from 'rxjs';

import { AuthService } from '../../core/services/auth';
import { InvoiceService } from '../../core/services/invoice';
import { SignalrService } from '../../core/services/signalr';
import { InvoiceListResponse } from '../../models/invoice.models';

@Component({
  selector: 'app-invoice-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './invoice-list.html',
  styleUrl: './invoice-list.scss'
})
export class InvoiceListComponent implements OnInit, OnDestroy {
  private invoiceService = inject(InvoiceService);
  private signalrService = inject(SignalrService);
  private authService = inject(AuthService);
  private zone = inject(NgZone);
  private cdr = inject(ChangeDetectorRef);

  invoices: InvoiceListResponse[] = [];

  search = '';
  pageNumber = 1;
  pageSize = 10;
  totalCount = 0;
  totalPages = 0;
  hasPreviousPage = false;
  hasNextPage = false;

  isLoading = false;
  errorMessage = '';

  deletingIds: number[] = [];

  ngOnInit(): void {
    this.loadInvoices();

    this.signalrService.startConnection().then(() => {
      this.signalrService.onInvoicesUpdated(() => {
        this.zone.run(() => {
          this.loadInvoices();
        });
      });
    });
  }

  ngOnDestroy(): void {
    this.signalrService.stopConnection();
  }

  loadInvoices(): void {
    this.isLoading = true;
    this.errorMessage = '';
    this.cdr.detectChanges();

    this.invoiceService
      .getAll(this.pageNumber, this.pageSize, this.search)
      .pipe(
        timeout(10000),
        finalize(() => {
          this.isLoading = false;
          this.cdr.detectChanges();
        })
      )
      .subscribe({
        next: (response) => {
          console.log('INVOICES RESPONSE:', response);

          if (!response.success || !response.data) {
            this.errorMessage = response.message || 'Failed to load invoices.';
            return;
          }

          this.invoices = response.data.items || [];
          this.pageNumber = response.data.pageNumber;
          this.pageSize = response.data.pageSize;
          this.totalCount = response.data.totalCount;
          this.totalPages = response.data.totalPages;
          this.hasPreviousPage = response.data.hasPreviousPage;
          this.hasNextPage = response.data.hasNextPage;
        },
        error: (error: any) => {
          console.log('LOAD INVOICES ERROR:', error);

          this.errorMessage =
            error?.error?.message ||
            error?.message ||
            'Failed to load invoices.';
        }
      });
  }

  searchInvoices(): void {
    this.pageNumber = 1;
    this.loadInvoices();
  }

  clearSearch(): void {
    this.search = '';
    this.pageNumber = 1;
    this.loadInvoices();
  }

  goToPage(page: number): void {
    if (page < 1 || page > this.totalPages) {
      return;
    }

    this.pageNumber = page;
    this.loadInvoices();
  }

  deleteInvoice(id: number): void {
    if (this.deletingIds.includes(id)) {
      return;
    }

    const confirmed = confirm('Are you sure you want to delete this invoice?');

    if (!confirmed) {
      return;
    }

    this.deletingIds.push(id);

    this.invoiceService.delete(id)
      .pipe(
        finalize(() => {
          this.deletingIds = this.deletingIds.filter(x => x !== id);
          this.cdr.detectChanges();
        })
      )
      .subscribe({
        next: () => {
          this.invoices = this.invoices.filter(invoice => invoice.id !== id);

          if (this.totalCount > 0) {
            this.totalCount--;
          }

          this.loadInvoices();
        },
        error: (error: any) => {
          console.log('DELETE ERROR:', error);

          this.errorMessage =
            error?.error?.message || 'Failed to delete invoice.';
        }
      });
  }

  isDeleting(id: number): boolean {
    return this.deletingIds.includes(id);
  }

  logout(): void {
    this.authService.logout();
  }

  get pages(): number[] {
    const result: number[] = [];

    for (let i = 1; i <= this.totalPages; i++) {
      result.push(i);
    }

    return result;
  }
}