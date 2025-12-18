import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AlertService } from '../../ui-service/alert.service';
import { ErrorHandlerService } from '../error-handler.service';
import { BehaviorSubject } from 'rxjs';
import {
  BookAllData,
  BookListResponse,
  BPaginationInfo,
} from '../../models/book';
import { ApiResponse } from '../../models/apiResponse';

@Injectable({
  providedIn: 'root',
})
export class UserWishListService {
  private readonly apiUrl = 'api/user/wishlist';

  constructor(
    private http: HttpClient,
    private alertService: AlertService,
    private errorHandler: ErrorHandlerService
  ) {}

  wishlist = new BehaviorSubject<BookAllData[]>([]);
  pagination = new BehaviorSubject<BPaginationInfo>({} as BPaginationInfo);

  getUserWishlist(pageNumber: number = 1, pageSize: number = 20) {
    const params = new HttpParams()
      .set('PageNumber', pageNumber.toString())
      .set('PageSize', pageSize.toString());

    this.http
      .get<ApiResponse<BookListResponse>>(`${this.apiUrl}`, { params })
      .subscribe({
        next: (res) => {
          this.wishlist.next(res.data?.books ?? []);
          this.pagination.next(res.data?.pagination ?? ({} as BPaginationInfo));
        },
        error: (err) => {
          this.wishlist.next([]);
          this.pagination.next({} as BPaginationInfo);
          this.errorHandler.handleError(err);
        },
      });
  }

  ToggleWishlist(bookId: number) {
    this.http
      .post<ApiResponse<boolean>>(`${this.apiUrl}`, { bookId })
      .subscribe({
        next: (res) => {
          if (res.data) {
            this.alertService.show(
              'افزودن به علاقه‌مندی با موفقیت انجام شد',
              'success'
            );
            const current = this.pagination.value;
            this.pagination.next({
              ...current,
              totalCount: (current.totalCount || 0) + 1,
            });
          } else {
            this.wishlist.next(
              this.wishlist.value.filter((b) => b.id !== bookId)
            );
            this.alertService.show(
              'حذف از علاقه‌مندی با موفقیت انجام شد',
              'error'
            );
            const current = this.pagination.value;
            this.pagination.next({
              ...current,
              totalCount: (current.totalCount || 0) - 1,
            });
          }
        },
        error: (err) => {
          this.errorHandler.handleError(err);
        },
      });
  }
}
