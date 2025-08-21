import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { AlertService } from '../../ui-service/alert.service';
import { ErrorHandlerService } from '../error-handler.service';
import { BehaviorSubject } from 'rxjs';
import { BookAllData, BookListResponse, BPaginationInfo } from '../../models/book';
import { ApiResponse } from '../../models/apiResponse';

@Injectable({
  providedIn: 'root',
})
export class BookService {
  private readonly apiUrl = 'api/admin/book';

  constructor(
    private http: HttpClient,
    private alertService: AlertService,
    private errorHandler: ErrorHandlerService
  ) {}

  books = new BehaviorSubject<BookAllData[]>([]);
  book = new BehaviorSubject<BookAllData>({} as BookAllData);
  pagination = new BehaviorSubject<BPaginationInfo>({
    pageNumber: 1,
    pageSize: 20,
    totalCount: 0,
    totalPages: 1,
  });
  createErrors = signal<string[]>([]);
  updateErrors = signal<string[]>([]);
  created = signal<boolean>(false);
  updated = signal<boolean>(false);

  getBooks(pageNumber: number = 1, pageSize: number = 20) {
    const params = new HttpParams()
      .set('PageNumber', pageNumber.toString())
      .set('PageSize', pageSize.toString());
    this.http
      .get<ApiResponse<BookListResponse>>(`${this.apiUrl}`, { params })
      .subscribe({
        next: (response) => {
          this.books.next([...(response.data?.books ?? [])]);
          this.pagination.next(response.data?.pagination!);
        },
        error: (err) => {
          this.errorHandler.handleError(err);
        },
      });
  }

  getById(id: number) {
    this.http.get<ApiResponse<BookAllData>>(`${this.apiUrl}/${id}`).subscribe({
      next: (res) => {
        this.book.next(res.data!);
      },
      error: (err) => {
        this.errorHandler.handleError(err);
      },
    });
  }

  create(book: BookAllData) {
    this.http.post<ApiResponse<BookAllData>>(`${this.apiUrl}`, book).subscribe({
      next: (res) => {
        this.books.next([res.data!, ...this.books.value]);
        this.createErrors.set([]); // clear errors
        this.created.set(true); // emit created book
        this.alertService.show('کتاب با موفقیت ایجاد شد', 'success');
      },
      error: (err) => {
        this.created.set(false);
        this.createErrors.set(this.errorHandler.handleError(err));
      },
    });
  }

  update(book: BookAllData, id: number) {
    this.http
      .put<ApiResponse<BookAllData>>(`${this.apiUrl}/${id}`, book)
      .subscribe({
        next: (res) => {
          this.books.next(
            this.books.value.map((a) =>
              a.id === res.data!.id ? res.data! : a
            )
          );
          this.updateErrors.set([]); // clear errors
          this.updated.set(true); // emit updated book
          this.alertService.show('کتاب با موفقیت به‌روزرسانی شد', 'success');
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
        this.books.next(this.books.value.filter((a) => a.id !== id));
        this.alertService.show('کتاب با موفقیت حذف شد', 'success');
      },
      error: (err) => {
        this.errorHandler.handleError(err);
      },
    });
  }
}
