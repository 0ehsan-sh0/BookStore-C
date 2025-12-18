import { HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ApiResponse } from '../../models/apiResponse';
import {
  Comment,
  CommentListResponse,
  COPaginationInfo,
} from '../../models/comment';
import { BasePublicService } from './base-public.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Injectable({
  providedIn: 'root',
})
export class CommentPublicService extends BasePublicService<
  any,
  Comment,
  CommentListResponse,
  COPaginationInfo
> {
  protected override readonly apiUrl = 'api/book';

  constructor() {
    super({
      pageNumber: 1,
      pageSize: 20,
      totalCount: 0,
      totalPages: 1,
    });
  }

  getBookComments(id: number, pageNumber: number = 1, pageSize: number = 20) {
    const params = new HttpParams()
      .set('PageNumber', pageNumber.toString())
      .set('PageSize', pageSize.toString());

    this.http
      .get<ApiResponse<CommentListResponse>>(`${this.apiUrl}/${id}/comments`, {
        params,
      })
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (res) => {
          if (res.data) {
            this.itemsSig.set(res.data.comments ?? []);
            if (res.data.pagination) {
              this.paginationSig.set(res.data.pagination);
            }
          }
        },
        error: (err) => {
          this.errorHandler.handleError(err);
        },
      });
  }
}
