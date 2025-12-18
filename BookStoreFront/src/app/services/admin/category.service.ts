import { Injectable } from '@angular/core';
import {
  Category,
  CategoryListResponse,
  CPaginationInfo,
} from '../../models/category';
import { BaseAdminService } from './base-admin.service';

@Injectable({
  providedIn: 'root',
})
export class CategoryService extends BaseAdminService<
  Category,
  CategoryListResponse,
  CPaginationInfo
> {
  protected readonly apiUrl = 'api/admin/category';
  protected readonly entityName = 'دسته‌بندی';

  categories = this.items;
  category = this.item;

  constructor() {
    super({
      pageNumber: 1,
      pageSize: 20,
      totalCount: 0,
      totalPages: 1,
    });
  }

  getCategories(
    pageNumber: number = 1,
    pageSize: number = 20,
    search: string = ''
  ) {
    this.getAll(pageNumber, pageSize, search);
  }

  protected getItemsFromResponse(
    response: CategoryListResponse
  ): Category[] | undefined {
    return response.categories;
  }

  protected getPaginationFromResponse(
    response: CategoryListResponse
  ): CPaginationInfo | undefined {
    return response.pagination;
  }
}
