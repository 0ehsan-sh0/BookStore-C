import { Injectable, signal } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import {
  UPaginationInfo,
  User,
  UserListResponse,
  UserRole,
} from '../../models/user';
import { BaseAdminService } from './base-admin.service';
import { ApiResponse } from '../../models/apiResponse';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Injectable({
  providedIn: 'root',
})
export class UserService extends BaseAdminService<
  User,
  UserListResponse,
  UPaginationInfo
> {
  protected readonly apiUrl = 'api/admin/user';
  protected readonly entityName = 'کاربر';

  users = this.items;
  user = this.item;

  private roleSig = signal<UserRole | null>(null);

  constructor() {
    super({
      pageNumber: 1,
      pageSize: 20,
      totalCount: 0,
      totalPages: 1,
    });
  }

  getUsers(
    pageNumber: number = 1,
    pageSize: number = 20,
    search: string = '',
    role: UserRole | null = null
  ) {
    this.roleSig.set(role);
    this.getAll(pageNumber, pageSize, search);
  }

  protected override getExtraParams(): HttpParams {
    const role = this.roleSig();
    return new HttpParams().set('Role', role !== null ? role.toString() : '');
  }

  protected getItemsFromResponse(
    response: UserListResponse
  ): User[] | undefined {
    return response.users;
  }

  protected getPaginationFromResponse(
    response: UserListResponse
  ): UPaginationInfo | undefined {
    return response.pagination;
  }
}
