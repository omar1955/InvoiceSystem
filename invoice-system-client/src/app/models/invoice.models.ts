export interface CreateInvoiceItemRequest {
  productId: number;
  quantity: number;
  discountPercentage: number;
  taxPercentage: number;
}

export interface CreateInvoiceRequest {
  storeId: number;
  customerId: number;
  note?: string;
  items: CreateInvoiceItemRequest[];
}

export interface UpdateInvoiceRequest {
  storeId: number;
  customerId: number;
  note?: string;
  items: CreateInvoiceItemRequest[];
}

export interface InvoiceItemResponse {
  id: number;
  productId: number;
  productName: string;
  quantity: number;
  price: number;
  discountPercentage: number;
  taxPercentage: number;
  total: number;
}

export interface InvoiceListResponse {
  id: number;
  serial: string;
  invoiceDate: string;
  customerName: string;
  storeName: string;
  totalPrice: number;
}

export interface InvoiceResponse {
  id: number;
  serial: string;
  invoiceDate: string;
  note?: string;
  storeId: number;
  storeName: string;
  customerId: number;
  customerName: string;
  totalPrice: number;
  items: InvoiceItemResponse[];
}