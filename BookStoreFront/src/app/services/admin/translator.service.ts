import {
  HttpClient,
  HttpErrorResponse,
  HttpParams,
} from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { AlertService } from '../../ui-service/alert.service';
import { BehaviorSubject } from 'rxjs';
import {
  CreateTranslatorRequest,
  TPaginationInfo,
  Translator,
  TranslatorListResponse,
  UpdateTranslatorRequest,
} from '../../models/translator';
import { ApiResponse } from '../../models/apiResponse';
import { ErrorHandlerService } from '../error-handler.service';

@Injectable({
  providedIn: 'root',
})
export class TranslatorService {
  private readonly apiUrl = 'api/admin/translator';

  constructor(
    private http: HttpClient,
    private alertService: AlertService,
    private errorHandler: ErrorHandlerService
  ) {}

  translators = new BehaviorSubject<Translator[]>([]);
  translator = new BehaviorSubject<Translator>({} as Translator);
  pagination = new BehaviorSubject<TPaginationInfo>({
    pageNumber: 1,
    pageSize: 20,
    totalCount: 0,
    totalPages: 1,
  });
  createErrors = signal<string[]>([]);
  updateErrors = signal<string[]>([]);
  created = signal<boolean>(false);
  updated = signal<boolean>(false);

  getTranslators(pageNumber: number = 1, pageSize: number = 20, search: string = '') {
    const params = new HttpParams()
      .set('PageNumber', pageNumber.toString())
      .set('PageSize', pageSize.toString())
      .set('Search', search);
    this.http
      .get<ApiResponse<TranslatorListResponse>>(`${this.apiUrl}`, { params })
      .subscribe({
        next: (response) => {
          this.translators.next([...(response.data?.translators ?? [])]);
          this.pagination.next(response.data?.pagination!);
        },
        error: (err) => {
          this.errorHandler.handleError(err);
        },
      });
  }

  create(translator: CreateTranslatorRequest) {
    this.http
      .post<ApiResponse<Translator>>(`${this.apiUrl}`, translator)
      .subscribe({
        next: (res) => {
          this.translators.next([res.data!, ...this.translators.value]);
          this.createErrors.set([]); // clear errors
          this.created.set(true); // emit created translator
          this.alertService.show('مترجم با موفقیت ایجاد شد', 'success');
        },
        error: (err) => {
          this.created.set(false);
          this.createErrors.set(this.errorHandler.handleError(err));
        },
      });
  }

  update(author: UpdateTranslatorRequest, id: number) {
    this.http
      .put<ApiResponse<Translator>>(`${this.apiUrl}/${id}`, author)
      .subscribe({
        next: (res) => {
          this.translators.next(
            this.translators.value.map((a) =>
              a.id === res.data!.id ? res.data! : a
            )
          );
          this.updateErrors.set([]); // clear errors
          this.updated.set(true); // emit updated author
          this.alertService.show('مترجم با موفقیت به‌روزرسانی شد', 'success');
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
        this.translators.next(
          this.translators.value.filter((a) => a.id !== id)
        );
        this.alertService.show('مترجم با موفقیت حذف شد', 'success');
      },
      error: (err) => {
        this.errorHandler.handleError(err);
      },
    });
  }

  getById(id: number) {
    this.http.get<ApiResponse<Translator>>(`${this.apiUrl}/${id}`).subscribe({
      next: (res) => {
        this.translator.next(res.data!);
      },
      error: (err) => {
        this.errorHandler.handleError(err);
      },
    });
  }
}
