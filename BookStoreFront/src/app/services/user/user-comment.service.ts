import { HttpClient } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { AlertService } from '../../ui-service/alert.service';
import { ErrorHandlerService } from '../error-handler.service';
import { BehaviorSubject } from 'rxjs';
import { Comment, CreateCommentRequest } from '../../models/comment';
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

  comment = new BehaviorSubject<Comment>({} as Comment);

  createErrors = signal<string[]>([]);
  created = signal<boolean>(false);

  create(bookId : number, comment: CreateCommentRequest) {
    this.http.post<ApiResponse<Comment>>(`${this.apiUrl}/${bookId}`,  comment ).subscribe({
      next: (res) => {
        this.comment.next(res.data ?? {} as Comment);
        this.createErrors.set([]); // clear errors
        this.created.set(true); // emit created comment
        this.alertService.show('نظر شما با موفقیت ثبت شد.پس از تایید مدیر نمایش داده میشود.', 'success');
      },
      error: (err) => {
        this.created.set(false);
        this.createErrors.set(this.errorHandler.handleError(err));
      },
    });
  }
}
