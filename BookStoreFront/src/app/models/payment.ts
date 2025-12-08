import { PaginationInfo } from "./apiResponse";
import { PaymentStatus } from "./enum";
import { Invoice } from "./invoice";


export interface Payment {
  id: number;
  invoiceId: number;
  gatewayId: string;
  price: number;
  paymentGateway: string;
  responseCode: string;
  message?: string | null;
  status: PaymentStatus;
  transactionCode?: string | null;
  createdAt?: Date;
  updatedAt?: Date;

  invoice?: Invoice | null;
}

export interface PaymentPaginationInfo extends PaginationInfo {}

export interface PaymentListResponse {
  payments: Payment[];
  pagination: PaymentPaginationInfo;
}
