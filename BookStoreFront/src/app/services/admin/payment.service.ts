import { Injectable } from '@angular/core';
import {
  Payment,
  PaymentListResponse,
  PaymentPaginationInfo,
} from '../../models/payment';
import { BaseAdminService } from './base-admin.service';

@Injectable({
  providedIn: 'root',
})
export class PaymentService extends BaseAdminService<
  Payment,
  PaymentListResponse,
  PaymentPaginationInfo
> {
  protected readonly apiUrl = 'api/admin/payment';
  protected readonly entityName = 'پرداخت';

  payments = this.items;

  constructor() {
    super({
      pageNumber: 1,
      pageSize: 20,
      totalCount: 0,
      totalPages: 1,
    });
  }

  getPayments(pageNumber: number, pageSize: number, search: string) {
    this.getAll(pageNumber, pageSize, search);
  }

  protected getItemsFromResponse(
    response: PaymentListResponse
  ): Payment[] | undefined {
    return response.payments;
  }

  protected getPaginationFromResponse(
    response: PaymentListResponse
  ): PaymentPaginationInfo | undefined {
    return response.pagination;
  }
}
