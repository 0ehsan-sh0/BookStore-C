import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { AlertService } from '../../ui-service/alert.service';
import { ErrorHandlerService } from '../error-handler.service';
import { BehaviorSubject } from 'rxjs';
import {
  Address,
  AddressListResponse,
  AddressPaginationInfo,
  CreateAddressRequest,
  UpdateAddressRequest,
} from '../../models/address';
import { ApiResponse } from '../../models/apiResponse';

@Injectable({
  providedIn: 'root',
})
export class UserAddressService {
  private readonly apiUrl = 'api/user/address';

  constructor(
    private http: HttpClient,
    private alertService: AlertService,
    private errorHandler: ErrorHandlerService
  ) {}

  addresses = new BehaviorSubject<Address[] | null>(null);
  addressPagination = new BehaviorSubject<AddressPaginationInfo | null>(null);

  address = new BehaviorSubject<Address | null>(null);
  createErrors = signal<string[]>([]);
  updateErrors = signal<string[]>([]);
  created = signal<boolean>(false);
  updated = signal<boolean>(false);

  getUserAddresses(pageNumber: number = 1, pageSize: number = 10) {
    const params = new HttpParams()
      .set('PageNumber', pageNumber.toString())
      .set('PageSize', pageSize.toString());

    this.http
      .get<ApiResponse<AddressListResponse>>(`${this.apiUrl}`, { params })
      .subscribe({
        next: (response) => {
          this.addresses.next(response.data?.addresses as Address[]);
          this.addressPagination.next(
            response.data?.pagination as AddressPaginationInfo
          );
        },
        error: (err) => {
          this.errorHandler.handleError(err);
        },
      });
  }

  create(address: CreateAddressRequest) {
    this.http.post<ApiResponse<Address>>(`${this.apiUrl}`, address).subscribe({
      next: (res) => {
        this.address.next(res.data as Address);
        this.addresses.next([res.data!, ...this.addresses.value!]);

        this.created.set(true); // emit created address
        this.alertService.show('آدرس با موفقیت ایجاد شد', 'success');
      },
      error: (err) => {
        this.created.set(false);
        this.createErrors.set(this.errorHandler.handleError(err));
      },
    });
  }

  update(address: UpdateAddressRequest, id: number) {
    this.http
      .put<ApiResponse<Address>>(`${this.apiUrl}/${id}`, address)
      .subscribe({
        next: (res) => {
          this.address.next(res.data as Address);
          this.updated.set(true); // emit updated address
          this.alertService.show('آدرس با موفقیت به‌روزرسانی شد', 'success');
        },
        error: (err) => {
          this.updated.set(false);
          this.updateErrors.set(this.errorHandler.handleError(err));
        },
      });
  }
}
