import { Injectable } from '@angular/core';
import { Tag, TagListResponse, TagPaginationInfo } from '../../models/tag';
import { BaseAdminService } from './base-admin.service';

@Injectable({
  providedIn: 'root',
})
export class TagService extends BaseAdminService<
  Tag,
  TagListResponse,
  TagPaginationInfo
> {
  protected readonly apiUrl = 'api/admin/tag';
  protected readonly entityName = 'تگ';

  tags = this.items;
  tag = this.item;

  constructor() {
    super({
      pageNumber: 1,
      pageSize: 20,
      totalCount: 0,
      totalPages: 1,
    });
  }

  getTags(pageNumber: number = 1, pageSize: number = 20, search: string = '') {
    this.getAll(pageNumber, pageSize, search);
  }

  protected getItemsFromResponse(response: TagListResponse): Tag[] | undefined {
    return response.tags;
  }

  protected getPaginationFromResponse(
    response: TagListResponse
  ): TagPaginationInfo | undefined {
    return response.pagination;
  }
}
