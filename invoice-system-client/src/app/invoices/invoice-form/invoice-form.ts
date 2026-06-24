import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { finalize, timeout } from 'rxjs';

import { CustomerService } from '../../core/services/customer';
import { InvoiceService } from '../../core/services/invoice';
import { ProductService } from '../../core/services/product';
import { StoreService } from '../../core/services/store';

import { CustomerResponse, ProductResponse, StoreResponse } from '../../models/lookup.models';
import { CreateInvoiceRequest, UpdateInvoiceRequest } from '../../models/invoice.models';

interface InvoiceFormItem {
  productId: number;
  quantity: number;
  price: number;
  discountPercentage: number;
  taxPercentage: number;
  total: number;
}

@Component({
  selector: 'app-invoice-form',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './invoice-form.html',
  styleUrl: './invoice-form.scss'
})
export class InvoiceFormComponent implements OnInit {
  private invoiceService = inject(InvoiceService);
  private productService = inject(ProductService);
  private storeService = inject(StoreService);
  private customerService = inject(CustomerService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private cdr = inject(ChangeDetectorRef);

  products: ProductResponse[] = [];
  stores: StoreResponse[] = [];
  customers: CustomerResponse[] = [];

  invoiceId: number | null = null;
  isEditMode = false;

  serial = '';
  invoiceDate = '';
  storeId = 0;
  customerId = 0;
  note = '';

  items: InvoiceFormItem[] = [];

  isLoading = false;
  isSaving = false;
  errorMessage = '';
  successMessage = '';

  ngOnInit(): void {
    this.loadLookups();

    const id = this.route.snapshot.paramMap.get('id');

    if (id) {
      this.invoiceId = Number(id);
      this.isEditMode = true;
      this.loadInvoice(this.invoiceId);
      return;
    }

    this.addItem();
  }

  loadLookups(): void {
    this.productService.getAll()
      .pipe(timeout(10000))
      .subscribe({
        next: (response) => {
          this.products = response.data || [];
          this.cdr.detectChanges();
        },
        error: (error: any) => {
          console.log('LOAD PRODUCTS ERROR:', error);
          this.errorMessage =
            error?.error?.message ||
            error?.message ||
            'Failed to load products.';
          this.cdr.detectChanges();
        }
      });

    this.storeService.getAll()
      .pipe(timeout(10000))
      .subscribe({
        next: (response) => {
          this.stores = response.data || [];
          this.cdr.detectChanges();
        },
        error: (error: any) => {
          console.log('LOAD STORES ERROR:', error);
          this.errorMessage =
            error?.error?.message ||
            error?.message ||
            'Failed to load stores.';
          this.cdr.detectChanges();
        }
      });

    this.customerService.getAll()
      .pipe(timeout(10000))
      .subscribe({
        next: (response) => {
          this.customers = response.data || [];
          this.cdr.detectChanges();
        },
        error: (error: any) => {
          console.log('LOAD CUSTOMERS ERROR:', error);
          this.errorMessage =
            error?.error?.message ||
            error?.message ||
            'Failed to load customers.';
          this.cdr.detectChanges();
        }
      });
  }

  loadInvoice(id: number): void {
    this.isLoading = true;
    this.errorMessage = '';
    this.cdr.detectChanges();

    this.invoiceService.getById(id)
      .pipe(
        timeout(10000),
        finalize(() => {
          this.isLoading = false;
          this.cdr.detectChanges();
        })
      )
      .subscribe({
        next: (response) => {
          console.log('INVOICE DETAILS RESPONSE:', response);

          if (!response.success || !response.data) {
            this.errorMessage = response.message || 'Failed to load invoice.';
            return;
          }

          const invoice = response.data;

          this.serial = invoice.serial;
          this.invoiceDate = invoice.invoiceDate;
          this.storeId = Number(invoice.storeId);
          this.customerId = Number(invoice.customerId);
          this.note = invoice.note || '';

          this.items = invoice.items.map(item => ({
            productId: Number(item.productId),
            quantity: Number(item.quantity),
            price: Number(item.price),
            discountPercentage: Number(item.discountPercentage),
            taxPercentage: Number(item.taxPercentage),
            total: Number(item.total)
          }));

          if (this.items.length === 0) {
            this.addItem();
          }

          this.calculateAllTotals();
        },
        error: (error: any) => {
          console.log('LOAD INVOICE DETAILS ERROR:', error);

          this.errorMessage =
            error?.error?.message ||
            error?.message ||
            'Failed to load invoice.';
        }
      });
  }

  addItem(): void {
    this.items.push({
      productId: 0,
      quantity: 1,
      price: 0,
      discountPercentage: 0,
      taxPercentage: 0,
      total: 0
    });

    this.cdr.detectChanges();
  }

  removeItem(index: number): void {
    if (this.items.length === 1) {
      return;
    }

    this.items.splice(index, 1);
    this.calculateAllTotals();
    this.cdr.detectChanges();
  }

  onProductChange(item: InvoiceFormItem): void {
    const product = this.products.find(p => p.id === Number(item.productId));

    if (!product) {
      item.price = 0;
      item.total = 0;
      this.cdr.detectChanges();
      return;
    }

    item.price = Number(product.price);
    this.calculateItemTotal(item);
    this.cdr.detectChanges();
  }

  calculateItemTotal(item: InvoiceFormItem): void {
    const quantity = Number(item.quantity) || 0;
    const price = Number(item.price) || 0;
    const discount = Number(item.discountPercentage) || 0;
    const tax = Number(item.taxPercentage) || 0;

    const subtotal = quantity * price;
    const discountValue = subtotal * discount / 100;
    const afterDiscount = subtotal - discountValue;
    const taxValue = afterDiscount * tax / 100;

    item.total = afterDiscount + taxValue;
  }

  calculateAllTotals(): void {
    this.items.forEach(item => this.calculateItemTotal(item));
  }

  get invoiceTotal(): number {
    return this.items.reduce((sum, item) => sum + Number(item.total || 0), 0);
  }

  submit(): void {
    this.errorMessage = '';
    this.successMessage = '';

    if (this.isSaving) {
      return;
    }

    if (!this.storeId) {
      this.errorMessage = 'Please select store.';
      return;
    }

    if (!this.customerId) {
      this.errorMessage = 'Please select customer.';
      return;
    }

    if (this.items.some(i => !i.productId || i.quantity <= 0)) {
      this.errorMessage = 'Please select product and valid quantity for all items.';
      return;
    }

    const request: CreateInvoiceRequest | UpdateInvoiceRequest = {
      storeId: Number(this.storeId),
      customerId: Number(this.customerId),
      note: this.note,
      items: this.items.map(item => ({
        productId: Number(item.productId),
        quantity: Number(item.quantity),
        discountPercentage: Number(item.discountPercentage),
        taxPercentage: Number(item.taxPercentage)
      }))
    };

    this.isSaving = true;
    this.cdr.detectChanges();

    if (this.isEditMode && this.invoiceId) {
      this.invoiceService.update(this.invoiceId, request)
        .pipe(
          timeout(10000),
          finalize(() => {
            this.isSaving = false;
            this.cdr.detectChanges();
          })
        )
        .subscribe({
          next: () => {
            this.router.navigateByUrl('/invoices');
          },
          error: (error: any) => {
            console.log('UPDATE INVOICE ERROR:', error);

            this.errorMessage =
              error?.error?.message ||
              error?.message ||
              'Failed to update invoice.';
          }
        });

      return;
    }

    this.invoiceService.create(request)
      .pipe(
        timeout(10000),
        finalize(() => {
          this.isSaving = false;
          this.cdr.detectChanges();
        })
      )
      .subscribe({
        next: () => {
          this.router.navigateByUrl('/invoices');
        },
        error: (error: any) => {
          console.log('CREATE INVOICE ERROR:', error);

          this.errorMessage =
            error?.error?.message ||
            error?.message ||
            'Failed to create invoice.';
        }
      });
  }
}