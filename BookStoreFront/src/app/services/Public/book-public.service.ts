import { HttpParams } from '@angular/common/http';
import { Injectable, signal, computed } from '@angular/core';
import {
  BookAllData,
  BookListResponse,
  BPaginationInfo,
} from '../../models/book';
import { BasePublicService } from './base-public.service';
import { ApiResponse } from '../../models/apiResponse';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Injectable({
  providedIn: 'root',
})
export class BookPublicService extends BasePublicService<
  BookAllData,
  BookAllData,
  BookListResponse,
  BPaginationInfo
> {
  protected override readonly apiUrl = 'api/book';

  private recommendedBooksSig = signal<BookAllData[]>([]);
  recommendedBooks = computed(() => this.recommendedBooksSig());

  constructor() {
    super({
      pageNumber: 1,
      pageSize: 20,
      totalCount: 0,
      totalPages: 1,
    });
    this.setupListingLogic();
  }

  protected override getItemsFromResponse(
    response: BookListResponse
  ): BookAllData[] | undefined {
    return response.books;
  }

  protected override getPaginationFromResponse(
    response: BookListResponse
  ): BPaginationInfo | undefined {
    return response.pagination;
  }

  getNewBooks(
    pageNumber: number = 1,
    pageSize: number = 20,
    isRecommended: boolean = false
  ) {
    if (isRecommended) {
      this.http
        .get<ApiResponse<BookListResponse>>(`${this.apiUrl}`, {
          params: new HttpParams()
            .set('PageNumber', '1')
            .set('PageSize', '10')
            .set('IsRecommended', 'true'),
        })
        .pipe(takeUntilDestroyed(this.destroyRef))
        .subscribe({
          next: (res) => {
            if (res.data?.books) {
              this.recommendedBooksSig.set(res.data.books);
            }
          },
          error: (err) => this.errorHandler.handleError(err),
        });
      return;
    }

    this.getAll(pageNumber, pageSize);
  }

  getBookById(bookId: number) {
    this.getDetails(bookId);
  }
}
