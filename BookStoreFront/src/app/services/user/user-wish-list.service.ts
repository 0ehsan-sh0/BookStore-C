import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal, inject, DestroyRef } from '@angular/core';
import { AlertService } from '../../ui-service/alert.service';
import { ErrorHandlerService } from '../error-handler.service';
import {
  BookAllData,
  BookListResponse,
  BPaginationInfo,
} from '../../models/book';
import { ApiResponse } from '../../models/apiResponse';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Injectable({
  providedIn: 'root',
})
export class UserWishListService {
  private readonly apiUrl = 'api/user/wishlist';
  private http = inject(HttpClient);
  private alertService = inject(AlertService);
  private errorHandler = inject(ErrorHandlerService);
  private destroyRef = inject(DestroyRef);

  public wishlist = signal<BookAllData[]>([]);
  public pagination = signal<BPaginationInfo>({
    pageNumber: 1,
    pageSize: 20,
    totalCount: 0,
    totalPages: 1,
  });

  getUserWishlist(pageNumber: number = 1, pageSize: number = 20) {
    const params = new HttpParams()
      .set('PageNumber', pageNumber.toString())
      .set('PageSize', pageSize.toString());

    this.http
      .get<ApiResponse<BookListResponse>>(`${this.apiUrl}`, { params })
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (res) => {
          this.wishlist.set(res.data?.books ?? []);
          this.pagination.set(
            res.data?.pagination ?? {
              pageNumber: 1,
              pageSize: 20,
              totalCount: 0,
              totalPages: 1,
            }
          );
        },
        error: (err) => {
          this.wishlist.set([]);
          this.errorHandler.handleError(err);
        },
      });
  }

  ToggleWishlist(bookId: number) {
    this.http
      .post<ApiResponse<boolean>>(`${this.apiUrl}`, { bookId })
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (res) => {
          if (res.data) {
            this.alertService.show(
              'افزودن به علاقه‌مندی با موفقیت انجام شد',
              'success'
            );
            const current = this.pagination();
            this.pagination.set({
              ...current,
              totalCount: (current.totalCount || 0) + 1,
            });
          } else {
            this.wishlist.update((list) => list.filter((b) => b.id !== bookId));
            this.alertService.show(
              'حذف از علاقه‌مندی با موفقیت انجام شد',
              'error'
            );
            const current = this.pagination();
            this.pagination.set({
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
