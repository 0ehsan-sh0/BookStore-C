import { Injectable } from '@angular/core';
import {
  APaginationInfo,
  Author,
  AuthorListResponse,
  CreateAuthorRequest,
  UpdateAuthorRequest,
} from '../../models/author';
import { BaseAdminService } from './base-admin.service';

@Injectable({
  providedIn: 'root',
})
export class AuthorService extends BaseAdminService<
  Author,
  AuthorListResponse,
  APaginationInfo
> {
  protected readonly apiUrl = 'api/admin/author';
  protected readonly entityName = 'نویسنده';

  authors = this.items;
  author = this.item;

  constructor() {
    super({
      pageNumber: 1,
      pageSize: 20,
      totalCount: 0,
      totalPages: 1,
    });
  }

  getAuthors(
    pageNumber: number = 1,
    pageSize: number = 20,
    search: string = ''
  ) {
    this.getAll(pageNumber, pageSize, search);
  }

  protected getItemsFromResponse(
    response: AuthorListResponse
  ): Author[] | undefined {
    return response.authors;
  }

  protected getPaginationFromResponse(
    response: AuthorListResponse
  ): APaginationInfo | undefined {
    return response.pagination;
  }
}
