import {
  HttpClient,
  HttpErrorResponse,
  HttpParams,
} from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { AlertService } from '../../ui-service/alert.service';
import { BehaviorSubject } from 'rxjs';
import {
  Category,
  CategoryListResponse,
  CPaginationInfo,
  CreateCategoryRequest,
  UpdateCategoryRequest,
} from '../../models/category';
import { ApiResponse } from '../../models/apiResponse';
import { ErrorHandlerService } from '../error-handler.service';

@Injectable({
  providedIn: 'root',
})
export class CategoryService {
  private readonly apiUrl = 'api/admin/category';

  constructor(
    private http: HttpClient,
    private alertService: AlertService,
    private errorHandler: ErrorHandlerService
  ) {}

  categories = new BehaviorSubject<Category[]>([]);
  category = new BehaviorSubject<Category>({} as Category);
  pagination = new BehaviorSubject<CPaginationInfo>({
    pageNumber: 1,
    pageSize: 20,
    totalCount: 0,
    totalPages: 1,
  });
  createErrors = signal<string[]>([]);
  updateErrors = signal<string[]>([]);
  created = signal<boolean>(false);
  updated = signal<boolean>(false);

  getCategories(pageNumber: number = 1, pageSize: number = 20) {
    const params = new HttpParams()
      .set('PageNumber', pageNumber.toString())
      .set('PageSize', pageSize.toString());
    this.http
      .get<ApiResponse<CategoryListResponse>>(`${this.apiUrl}`, { params })
      .subscribe({
        next: (response) => {
          this.categories.next([...(response.data?.categories ?? [])]);
          this.pagination.next(response.data?.pagination! ?? null);
        },
        error: (err) => {
          this.errorHandler.handleError(err);
        },
      });
  }

  getById(id: number) {
    this.http.get<ApiResponse<Category>>(`${this.apiUrl}/${id}`).subscribe({
      next: (res) => {
        this.category.next(res.data!);
      },
      error: (err) => {
        this.errorHandler.handleError(err);
      },
    });
  }

  create(category: CreateCategoryRequest) {
    this.http
      .post<ApiResponse<Category>>(`${this.apiUrl}`, category)
      .subscribe({
        next: (res) => {
          this.categories.next([res.data!, ...this.categories.value]);
          this.created.set(true);
          this.createErrors.set([]);
          this.alertService.show('دسته‌بندی با موفقیت ایجاد شد', 'success');
        },
        error: (err) => {
          this.createErrors.set(this.errorHandler.handleError(err));
        },
      });
  }

  update(category: UpdateCategoryRequest, id: number) {
    this.http
      .put<ApiResponse<Category>>(`${this.apiUrl}/${id}`, category)
      .subscribe({
        next: (res) => {
          this.categories.next(
            this.categories.value.map((a) =>
              a.id === res.data!.id ? res.data! : a
            )
          );
          this.updated.set(true);
          this.updateErrors.set([]);
          this.alertService.show(
            'دسته‌بندی با موفقیت به‌روزرسانی شد',
            'success'
          );
        },
        error: (err) => {
          this.updateErrors.set(this.errorHandler.handleError(err));
        },
      });
  }

  delete(id: number) {
    this.http.delete<null>(`${this.apiUrl}/${id}`).subscribe({
      next: () => {
        this.categories.next(this.categories.value.filter((c) => c.id !== id));
        this.alertService.show('دسته‌بندی با موفقیت حذف شد', 'success');
      },
      error: (err) => {
        this.errorHandler.handleError(err);
      },
    });
  }
}
