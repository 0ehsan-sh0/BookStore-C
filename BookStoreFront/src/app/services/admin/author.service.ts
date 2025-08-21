import {
  HttpClient,
  HttpErrorResponse,
  HttpParams,
} from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { AlertService } from '../../ui-service/alert.service';
import { BehaviorSubject } from 'rxjs';
import {
  APaginationInfo,
  Author,
  AuthorListResponse,
  CreateAuthorRequest,
  UpdateAuthorRequest,
} from '../../models/author';
import { ApiResponse } from '../../models/apiResponse';
import { toSignal } from '@angular/core/rxjs-interop';
import { ErrorHandlerService } from '../error-handler.service';

@Injectable({
  providedIn: 'root',
})
export class AuthorService {
  private readonly apiUrl = 'api/admin/author';

  constructor(
    private http: HttpClient,
    private alertService: AlertService,
    private errorHandler: ErrorHandlerService
  ) {}

  authors = new BehaviorSubject<Author[]>([]);
  author = new BehaviorSubject<Author>({} as Author);
  pagination = new BehaviorSubject<APaginationInfo>({
    pageNumber: 1,
    pageSize: 20,
    totalCount: 0,
    totalPages: 1,
  });
  createErrors = signal<string[]>([]);
  updateErrors = signal<string[]>([]);
  created = signal<boolean>(false);
  updated = signal<boolean>(false);

  getAuthors(pageNumber: number = 1, pageSize: number = 20) {
    const params = new HttpParams()
      .set('PageNumber', pageNumber.toString())
      .set('PageSize', pageSize.toString());
    this.http
      .get<ApiResponse<AuthorListResponse>>(`${this.apiUrl}`, { params })
      .subscribe({
        next: (response) => {
          this.authors.next([...(response.data?.authors ?? [])]);
          this.pagination.next(response.data?.pagination!);
        },
        error: (err) => {
          this.errorHandler.handleError(err);
        },
      });
  }

  create(author: CreateAuthorRequest) {
    this.http.post<ApiResponse<Author>>(`${this.apiUrl}`, author).subscribe({
      next: (res) => {
        this.authors.next([res.data!, ...this.authors.value]);
        this.createErrors.set([]); // clear errors
        this.created.set(true); // emit created author
        this.alertService.show('نویسنده با موفقیت ایجاد شد', 'success');
      },
      error: (err) => {
        this.created.set(false);
        this.createErrors.set(this.errorHandler.handleError(err));
      },
    });
  }

  update(author: UpdateAuthorRequest, id: number) {
    this.http
      .put<ApiResponse<Author>>(`${this.apiUrl}/${id}`, author)
      .subscribe({
        next: (res) => {
          this.authors.next(
            this.authors.value.map((a) =>
              a.id === res.data!.id ? res.data! : a
            )
          );
          this.updateErrors.set([]); // clear errors
          this.updated.set(true); // emit updated author
          this.alertService.show('نویسنده با موفقیت به‌روزرسانی شد', 'success');
        },
        error: (err) => {
          this.updated.set(false);
          this.updateErrors.set(this.errorHandler.handleError(err));
        },
      });
  }

  delete(id: number) {
    this.http.delete<null>(`${this.apiUrl}/${id}`).subscribe({
      next: () => {
        this.authors.next(this.authors.value.filter((a) => a.id !== id));
        this.alertService.show('نویسنده با موفقیت حذف شد', 'success');
      },
      error: (err) => {
        this.errorHandler.handleError(err);
      },
    });
  }

  getById(id: number) {
    this.http.get<ApiResponse<Author>>(`${this.apiUrl}/${id}`).subscribe({
      next: (res) => {
        this.author.next(res.data!);
      },
      error: (err) => {
        this.errorHandler.handleError(err);
      },
    });
  }

}
