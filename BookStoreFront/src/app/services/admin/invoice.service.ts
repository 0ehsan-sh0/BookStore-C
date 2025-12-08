import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AlertService } from '../../ui-service/alert.service';
import { ErrorHandlerService } from '../error-handler.service';
import { BehaviorSubject } from 'rxjs';
import { Invoice, InvoiceListResponse, InvoicePaginationInfo } from '../../models/invoice';
import { ApiResponse } from '../../models/apiResponse';

@Injectable({
  providedIn: 'root'
})
export class InvoiceService {
private readonly apiUrl = 'api/admin/invoice';

  constructor(
    private http: HttpClient,
    private alertService: AlertService,
    private errorHandler: ErrorHandlerService
  ) {}
  
  invoices = new BehaviorSubject<Invoice[]>([]);
  pagination = new BehaviorSubject<InvoicePaginationInfo>({
    pageNumber: 1,
    pageSize: 20,
    totalCount: 0,
    totalPages: 1,
  });

  getInvoices(pageNumber: number = 1, pageSize: number = 20, search: string = '') {
    const params = new HttpParams()
      .set('PageNumber', pageNumber.toString())
      .set('PageSize', pageSize.toString())
      .set('Search', search);
      
    this.http
      .get<ApiResponse<InvoiceListResponse>>(`${this.apiUrl}`, { params })
      .subscribe({
        next: (response) => {
          this.invoices.next(response.data?.invoices || []);
          this.pagination.next(response.data?.pagination!);
        },
        error: (err) => {
          this.errorHandler.handleError(err);
        },
      });
  }
}
