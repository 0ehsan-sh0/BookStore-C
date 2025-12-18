import { Injectable } from '@angular/core';
import { User } from '../../models/user';
import { ApiResponse } from '../../models/apiResponse';
import { HttpClient, HttpParams } from '@angular/common/http';
import { AlertService } from '../../ui-service/alert.service';
import { ErrorHandlerService } from '../error-handler.service';
import { BehaviorSubject } from 'rxjs';
import {
  Invoice,
  InvoicePaginationInfo,
  UserInvoicesList,
} from '../../models/invoice';
import {
  Address,
  AddressListResponse,
  AddressPaginationInfo,
} from '../../models/address';

@Injectable({
  providedIn: 'root',
})
export class UserPanelService {
  private readonly apiUrl = 'api/user';

  constructor(
    private http: HttpClient,
    private alertService: AlertService,
    private errorHandler: ErrorHandlerService
  ) {}

  user = new BehaviorSubject<User | null>(null);

  invoices = new BehaviorSubject<Invoice[] | null>(null);
  invoicePagination = new BehaviorSubject<InvoicePaginationInfo>(
    {} as InvoicePaginationInfo
  );
  userInvoice = new BehaviorSubject<Invoice | null>(null);

  updateUser(user: User) {
    this.http.put<ApiResponse<User>>(`${this.apiUrl}`, user).subscribe({
      next: (response) => {
        // The access token will be stored in a cookie by the backend
        this.user.next(response.data as User);
        // Update localStorage
        localStorage.setItem('user', JSON.stringify(response.data as User));
        this.alertService.show('اطلاعات شما با موفقیت به روز شد', 'success');
      },
      error: (err) => {
        this.errorHandler.handleError(err);
      },
    });
  }

  getUserInvoices(pageNumber: number = 1, pageSize: number = 10) {
    this.getInvoicesRequest(pageNumber, pageSize).subscribe({
      next: (response) => {
        this.invoices.next(response.data?.invoices as Invoice[]);
        this.invoicePagination.next(
          response.data?.pagination as InvoicePaginationInfo
        );
      },
      error: (err) => {
        this.errorHandler.handleError(err);
      },
    });
  }

  getInvoicesRequest(pageNumber: number, pageSize: number) {
    const params = new HttpParams()
      .set('PageNumber', pageNumber.toString())
      .set('PageSize', pageSize.toString());

    return this.http.get<ApiResponse<UserInvoicesList>>(
      `${this.apiUrl}/invoice`,
      { params }
    );
  }

  getUserInvoicesCount() {
    return this.getInvoicesRequest(1, 1);
  }

  getUserInvoice(id: number) {
    this.http
      .get<ApiResponse<Invoice>>(`${this.apiUrl}/invoice/${id}`)
      .subscribe({
        next: (response) => {
          this.userInvoice.next(response.data as Invoice);
        },
        error: (err) => {
          this.errorHandler.handleError(err);
        },
      });
  }
}
