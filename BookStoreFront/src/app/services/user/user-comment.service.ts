import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { AlertService } from '../../ui-service/alert.service';
import { ErrorHandlerService } from '../error-handler.service';
import { BehaviorSubject } from 'rxjs';
import {
  Comment,
  CommentListResponse,
  COPaginationInfo,
  CreateCommentRequest,
} from '../../models/comment';
import { ApiResponse } from '../../models/apiResponse';

@Injectable({
  providedIn: 'root',
})
export class UserCommentService {
  private readonly apiUrl = 'api/user/comment';

  constructor(
    private http: HttpClient,
    private alertService: AlertService,
    private errorHandler: ErrorHandlerService
  ) {}

  comments = new BehaviorSubject<Comment[]>([]);
  pagination = new BehaviorSubject<COPaginationInfo>({} as COPaginationInfo);
  comment = new BehaviorSubject<Comment>({} as Comment);

  createErrors = signal<string[]>([]);
  created = signal<boolean>(false);

  getUserComments(pageNumber: number, pageSize: number) {
    const params = new HttpParams()
      .set('PageNumber', pageNumber.toString())
      .set('PageSize', pageSize.toString());
    this.http
      .get<ApiResponse<CommentListResponse>>(`${this.apiUrl}`, { params })
      .subscribe({
        next: (res) => {
          this.comments.next(res.data?.comments ?? []);
          this.pagination.next(
            res.data?.pagination ?? ({} as COPaginationInfo)
          );
        },
        error: (err) => {
          this.comments.next([]);
          this.pagination.next({} as COPaginationInfo);
          this.errorHandler.handleError(err);
        },
      });
  }

  create(bookId: number, comment: CreateCommentRequest) {
    this.http
      .post<ApiResponse<Comment>>(`${this.apiUrl}/${bookId}`, comment)
      .subscribe({
        next: (res) => {
          this.comment.next(res.data ?? ({} as Comment));
          this.createErrors.set([]); // clear errors
          this.created.set(true); // emit created comment
          this.alertService.show(
            'نظر شما با موفقیت ثبت شد.پس از تایید مدیر نمایش داده میشود.',
            'success'
          );
        },
        error: (err) => {
          this.created.set(false);
          this.createErrors.set(this.errorHandler.handleError(err));
        },
      });
  }
}
