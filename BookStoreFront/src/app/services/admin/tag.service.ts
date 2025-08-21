import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { AlertService } from '../../ui-service/alert.service';
import { ErrorHandlerService } from '../error-handler.service';
import { BehaviorSubject } from 'rxjs';
import {
  CreateTagRequest,
  Tag,
  TagListResponse,
  TagPaginationInfo,
  UpdateTagRequest,
} from '../../models/tag';
import { ApiResponse } from '../../models/apiResponse';

@Injectable({
  providedIn: 'root',
})
export class TagService {
  private readonly apiUrl = 'api/admin/tag';

  constructor(
    private http: HttpClient,
    private alertService: AlertService,
    private errorHandler: ErrorHandlerService
  ) {}

  tags = new BehaviorSubject<Tag[]>([]);
  tag = new BehaviorSubject<Tag>({} as Tag);
  pagination = new BehaviorSubject<TagPaginationInfo>({
    pageNumber: 1,
    pageSize: 20,
    totalCount: 0,
    totalPages: 1,
  });
  createErrors = signal<string[]>([]);
  updateErrors = signal<string[]>([]);
  created = signal<boolean>(false);
  updated = signal<boolean>(false);

  getTags(pageNumber: number = 1, pageSize: number = 20) {
    const params = new HttpParams()
      .set('PageNumber', pageNumber.toString())
      .set('PageSize', pageSize.toString());
    this.http
      .get<ApiResponse<TagListResponse>>(`${this.apiUrl}`, { params })
      .subscribe({
        next: (response) => {
          this.tags.next([...(response.data?.tags ?? [])]);
          this.pagination.next(response.data?.pagination!);
        },
        error: (err) => {
          this.errorHandler.handleError(err);
        },
      });
  }

  getById(id: number) {
    this.http.get<ApiResponse<Tag>>(`${this.apiUrl}/${id}`).subscribe({
      next: (res) => {
        this.tag.next(res.data!);
      },
      error: (err) => {
        this.errorHandler.handleError(err);
      },
    });
  }

  create(tag: CreateTagRequest) {
    this.http.post<ApiResponse<Tag>>(`${this.apiUrl}`, tag).subscribe({
      next: (res) => {
        this.tags.next([res.data!, ...this.tags.value]);
        this.createErrors.set([]); // clear errors
        this.created.set(true); // emit created tag
        this.alertService.show('تگ با موفقیت ایجاد شد', 'success');
      },
      error: (err) => {
        this.created.set(false);
        this.createErrors.set(this.errorHandler.handleError(err));
      },
    });
  }

  update(tag: UpdateTagRequest, id: number) {
    this.http.put<ApiResponse<Tag>>(`${this.apiUrl}/${id}`, tag).subscribe({
      next: (res) => {
        this.tags.next(
          this.tags.value.map((a) => (a.id === res.data!.id ? res.data! : a))
        );
        this.updateErrors.set([]); // clear errors
        this.updated.set(true); // emit updated tag
        this.alertService.show('تگ با موفقیت به‌روزرسانی شد', 'success');
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
        this.tags.next(this.tags.value.filter((a) => a.id !== id));
        this.alertService.show('تگ با موفقیت حذف شد', 'success');
      },
      error: (err) => {
        this.errorHandler.handleError(err);
      },
    });
  }
}
