import { Injectable } from '@angular/core';
import {
  Comment,
  CommentListResponse,
  COPaginationInfo,
} from '../../models/comment';
import { BaseAdminService } from './base-admin.service';
import { ApiResponse } from '../../models/apiResponse';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Injectable({
  providedIn: 'root',
})
export class CommentService extends BaseAdminService<
  Comment,
  CommentListResponse,
  COPaginationInfo
> {
  protected readonly apiUrl = 'api/admin/comment';
  protected readonly entityName = 'نظر';

  comments = this.items;
  comment = this.item;

  constructor() {
    super({
      pageNumber: 1,
      pageSize: 20,
      totalCount: 0,
      totalPages: 1,
    });
  }

  getComments(
    pageNumber: number = 1,
    pageSize: number = 20,
    search: string = ''
  ) {
    this.getAll(pageNumber, pageSize, search);
  }

  protected getItemsFromResponse(
    response: CommentListResponse
  ): Comment[] | undefined {
    return response.comments;
  }

  protected getPaginationFromResponse(
    response: CommentListResponse
  ): COPaginationInfo | undefined {
    return response.pagination;
  }

  ChangeStatus(id: number) {
    this.http
      .post<ApiResponse<Comment>>(`${this.apiUrl}/status/${id}`, {})
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (res) => {
          if (res.data) {
            this.itemsSig.update((val) =>
              val.map((c: any) => (c.id === id ? res.data! : c))
            );
            this.alertService.show('وضعیت نظر با موفقیت تغییر کرد', 'success');
          }
        },
        error: (err) => {
          this.errorHandler.handleError(err);
        },
      });
  }
}
