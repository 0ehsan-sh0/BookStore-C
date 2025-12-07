import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ApiResponse } from '../../models/apiResponse';
import { Comment, CommentListResponse, COPaginationInfo } from '../../models/comment';
import { ErrorHandlerService } from '../error-handler.service';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CommentPublicService {

  comments = new BehaviorSubject<Comment[]>([]);
  pagination = new BehaviorSubject<COPaginationInfo>({} as COPaginationInfo);

  constructor(
    private http: HttpClient,
    private errorHandler: ErrorHandlerService
  ) { }

  getBookComments(id: number,pageNumber : number = 1, pageSize: number = 20) {
      const params = new HttpParams()
        .set('PageNumber', pageNumber.toString())
        .set('PageSize', pageSize.toString());
      this.http
        .get<ApiResponse<CommentListResponse>>(`api/book/${id}/comments`, { params })
        .subscribe({
          next: (res) => {
            this.comments.next(res.data?.comments!);
            this.pagination.next(res.data?.pagination!);
          },
          error: (err) => {
            this.errorHandler.handleError(err);
          },
        });
    }
}
