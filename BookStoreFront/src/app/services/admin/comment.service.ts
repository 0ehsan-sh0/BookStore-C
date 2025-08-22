import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { AlertService } from '../../ui-service/alert.service';
import { ErrorHandlerService } from '../error-handler.service';
import {
  Comment,
  CommentListResponse,
  COPaginationInfo,
} from '../../models/comment';
import { BehaviorSubject } from 'rxjs';
import { ApiResponse } from '../../models/apiResponse';

@Injectable({
  providedIn: 'root',
})
export class CommentService {
  private readonly apiUrl = 'api/admin/comment';
  constructor(
    private http: HttpClient,
    private alertService: AlertService,
    private errorHandler: ErrorHandlerService
  ) {}

  comments = new BehaviorSubject<Comment[]>([]);
  comment = new BehaviorSubject<Comment>({} as Comment);
  pagination = new BehaviorSubject<COPaginationInfo>({
    pageNumber: 1,
    pageSize: 20,
    totalCount: 0,
    totalPages: 1,
  });

  getComments(pageNumber: number = 1, pageSize: number = 20, search: string = '') {
    const params = new HttpParams()
      .set('PageNumber', pageNumber.toString())
      .set('PageSize', pageSize.toString())
      .set('Search', search);
    this.http
      .get<ApiResponse<CommentListResponse>>(`${this.apiUrl}`, { params })
      .subscribe({
        next: (response) => {
          this.comments.next(response.data?.comments ?? []);
          this.pagination.next(response.data?.pagination!);
        },
        error: (error) => {
          this.errorHandler.handleError(error);
        },
      });
  }

  getById(id: number) {
    this.http.get<ApiResponse<Comment>>(`${this.apiUrl}/${id}`).subscribe({
      next: (res) => {
        this.comment.next(res.data!);
      },
      error: (err) => {
        this.errorHandler.handleError(err);
      },
    });
  }

  ChangeStatus(id: number) {
    this.http
      .post<ApiResponse<Comment>>(`${this.apiUrl}/status/${id}`, {})
      .subscribe({
        next: (res) => {
          this.comments.next(
            this.comments.value.map((c) =>
              c.id === id ? res.data! : c
            )
          );
          this.alertService.show('وضعیت نظر با موفقیت تغییر کرد', 'success');
        },
        error: (err) => {
          this.errorHandler.handleError(err);
        },
      });
  }

  delete(id: number) {
    this.http.delete<null>(`${this.apiUrl}/${id}`).subscribe({
      next: () => {
        this.comments.next(this.comments.value.filter((c) => c.id !== id));
        this.alertService.show('نظر با موفقیت حذف شد', 'success');
      },
      error: (err) => {
        this.errorHandler.handleError(err);
      },
    });
  }
}
