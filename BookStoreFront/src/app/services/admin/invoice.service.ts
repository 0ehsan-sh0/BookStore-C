import { Injectable } from '@angular/core';
import {
  Invoice,
  InvoiceListResponse,
  InvoicePaginationInfo,
} from '../../models/invoice';
import { BaseAdminService } from './base-admin.service';

@Injectable({
  providedIn: 'root',
})
export class InvoiceService extends BaseAdminService<
  Invoice,
  InvoiceListResponse,
  InvoicePaginationInfo
> {
  protected readonly apiUrl = 'api/admin/invoice';
  protected readonly entityName = 'فاکتور';

  invoices = this.items;

  constructor() {
    super({
      pageNumber: 1,
      pageSize: 20,
      totalCount: 0,
      totalPages: 1,
    });
  }

  getInvoices(
    pageNumber: number = 1,
    pageSize: number = 20,
    search: string = ''
  ) {
    this.getAll(pageNumber, pageSize, search);
  }

  protected getItemsFromResponse(
    response: InvoiceListResponse
  ): Invoice[] | undefined {
    return response.invoices;
  }

  protected getPaginationFromResponse(
    response: InvoiceListResponse
  ): InvoicePaginationInfo | undefined {
    return response.pagination;
  }
}
