import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { AlertService } from '../../ui-service/alert.service';
import { ErrorHandlerService } from '../error-handler.service';
import { BehaviorSubject } from 'rxjs';
import {
  CreateUserRequest,
  UPaginationInfo,
  UpdateUserRequest,
  User,
  UserListResponse,
  UserRole,
} from '../../models/user';
import { ApiResponse } from '../../models/apiResponse';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private readonly apiUrl = 'api/admin/user';

  constructor(
    private http: HttpClient,
    private alertService: AlertService,
    private errorHandler: ErrorHandlerService
  ) {}

  users = new BehaviorSubject<User[]>([]);
  user = new BehaviorSubject<User>({} as User);
  pagination = new BehaviorSubject<UPaginationInfo>({
    pageNumber: 1,
    pageSize: 20,
    totalCount: 0,
    totalPages: 1,
  });
  createErrors = signal<string[]>([]);
  updateErrors = signal<string[]>([]);
  created = signal<boolean>(false);
  updated = signal<boolean>(false);

  getUsers(
    pageNumber: number = 1,
    pageSize: number = 20,
    search: string = '',
    role: UserRole | null = null
  ) {
    const params = new HttpParams()
      .set('PageNumber', pageNumber.toString())
      .set('PageSize', pageSize.toString())
      .set('Search', search)
      .set('Role', role !== null ? role.toString() : '');

    this.http
      .get<ApiResponse<UserListResponse>>(`${this.apiUrl}`, { params })
      .subscribe({
        next: (response) => {
          this.users.next([...(response.data?.users ?? [])]);
          this.pagination.next(response.data?.pagination!);
        },
        error: (err) => {
          this.errorHandler.handleError(err);
        },
      });
  }

  getById(id: number) {
    this.http.get<ApiResponse<User>>(`${this.apiUrl}/${id}`).subscribe({
      next: (response) => {
        this.user.next(response.data ?? ({} as User));
      },
      error: (err) => {
        this.errorHandler.handleError(err);
      },
    });
  }

  create(user: CreateUserRequest) {
    this.http.post<ApiResponse<User>>(`${this.apiUrl}`, user).subscribe({
      next: (response) => {
        this.users.next([response.data!, ...this.users.value]);
        this.createErrors.set([]); // clear errors
        this.created.set(true);
        this.alertService.show('کاربر ایجاد شد');
      },
      error: (err) => {
        this.created.set(false);
        this.createErrors.set(this.errorHandler.handleError(err));
      },
    });
  }

  update(user: UpdateUserRequest, id: number) {
    this.http.put<ApiResponse<User>>(`${this.apiUrl}/${id}`, user).subscribe({
      next: (response) => {
        this.users.next(
          this.users.value.map((u) => {
            if (u.id === id) {
              return response.data ?? u;
            }
            return u;
          })
        );
        this.updateErrors.set([]); // clear errors
        this.updated.set(true);
        this.alertService.show('کاربر به‌روزرسانی شد');
      },
      error: (err) => {
        this.updated.set(false);
        this.updateErrors.set(this.errorHandler.handleError(err));
      },
    });
  }

  delete(id: number) {
    this.http.delete(`${this.apiUrl}/${id}`).subscribe({
      next: () => {
        this.users.next(this.users.value.filter((u) => u.id !== id));
        this.alertService.show('کاربر حذف شد');
      },
      error: (err) => {
        this.errorHandler.handleError(err);
      },
    });
  }
}
