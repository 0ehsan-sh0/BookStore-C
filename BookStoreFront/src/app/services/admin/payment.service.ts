import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AlertService } from '../../ui-service/alert.service';
import { ErrorHandlerService } from '../error-handler.service';
import { BehaviorSubject } from 'rxjs';
import { Payment, PaymentListResponse, PaymentPaginationInfo } from '../../models/payment';
import { ApiResponse } from '../../models/apiResponse';

@Injectable({
  providedIn: 'root'
})
export class PaymentService {

  private readonly apiUrl = 'api/admin/payment';

  constructor(
    private http: HttpClient,
    private alertService: AlertService,
    private errorHandler: ErrorHandlerService
  ) {}

  payments = new BehaviorSubject<Payment[]>([]);
  pagination = new BehaviorSubject<PaymentPaginationInfo>({
    pageNumber: 1,
    pageSize: 20,
    totalCount: 0,
    totalPages: 1,
  });

  getPayments(pageNumber: number, pageSize: number, search: string) {
    // Implementation for fetching payments would go here
    const params = new HttpParams()
      .set('PageNumber', pageNumber.toString())
      .set('PageSize', pageSize.toString())
      .set('Search', search);

    this.http
      .get<ApiResponse<PaymentListResponse>>(`${this.apiUrl}`, { params })
      .subscribe({
        next: (response) => {
          this.payments.next(response.data?.payments as Payment[]);
          this.pagination.next(response.data?.pagination as PaymentPaginationInfo);
        },
        error: (err) => {
          this.errorHandler.handleError(err);
        },
      });
  }
}
