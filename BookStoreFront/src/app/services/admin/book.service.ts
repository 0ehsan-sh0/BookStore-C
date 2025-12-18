import { Injectable } from '@angular/core';
import {
  BookAllData,
  BookListResponse,
  BPaginationInfo,
} from '../../models/book';
import { BaseAdminService } from './base-admin.service';
import { ApiResponse } from '../../models/apiResponse';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Injectable({
  providedIn: 'root',
})
export class BookService extends BaseAdminService<
  BookAllData,
  BookListResponse,
  BPaginationInfo
> {
  protected readonly apiUrl = 'api/admin/book';
  protected readonly entityName = 'کتاب';

  books = this.items;
  book = this.item;

  constructor() {
    super({
      pageNumber: 1,
      pageSize: 20,
      totalCount: 0,
      totalPages: 1,
    });
  }

  getBooks(pageNumber: number = 1, pageSize: number = 20, search: string = '') {
    this.getAll(pageNumber, pageSize, search);
  }

  protected getItemsFromResponse(
    response: BookListResponse
  ): BookAllData[] | undefined {
    return response.books;
  }

  protected getPaginationFromResponse(
    response: BookListResponse
  ): BPaginationInfo | undefined {
    return response.pagination;
  }

  toggleRecomended(id: number) {
    return this.http
      .post<ApiResponse<BookAllData>>(`${this.apiUrl}/recommended/${id}`, null)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (res) => {
          if (res.data) {
            this.itemsSig.update((val) =>
              val.map((a) => (a.id === res.data!.id ? res.data! : a))
            );
            this.alertService.show(
              'وضعیت پیشنهاد کتاب با موفقیت تغییر کرد',
              'success'
            );
          }
        },
        error: (err) => {
          this.errorHandler.handleError(err);
        },
      });
  }
}
