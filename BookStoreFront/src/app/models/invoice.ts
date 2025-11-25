import { Address } from "./address";
import { PaginationInfo } from "./apiResponse";
import { BookAllData } from "./book";
import { InvoiceStatus, PaymentStatus } from "./enum";
import { Payment } from "./payment";
import { User } from "./user";

export interface Invoice {
  id: number;
  totalPrice: number;
  maliat: number;
  finalTotalPrice: number;
  paymentStatus: PaymentStatus;
  invoiceStatus: InvoiceStatus;
  userId: number;
  addressId: number;
  createdAt?: Date;
  updatedAt?: Date;
  validatedAt?: Date;

  books?: BookAllData[] | null;
  payments?: Payment[] | null;
  user?: User | null;
  address?: Address | null;
}

export interface UserInvoicesList {
  invoices: Invoice[] | null;
  pagination: InvoicePaginationInfo | null;
}

export interface InvoicePaginationInfo extends PaginationInfo {}