import { Injectable } from '@angular/core';
import {
  TPaginationInfo,
  Translator,
  TranslatorListResponse,
} from '../../models/translator';
import { BaseAdminService } from './base-admin.service';

@Injectable({
  providedIn: 'root',
})
export class TranslatorService extends BaseAdminService<
  Translator,
  TranslatorListResponse,
  TPaginationInfo
> {
  protected readonly apiUrl = 'api/admin/translator';
  protected readonly entityName = 'مترجم';

  translators = this.items;
  translator = this.item;

  constructor() {
    super({
      pageNumber: 1,
      pageSize: 20,
      totalCount: 0,
      totalPages: 1,
    });
  }

  getTranslators(
    pageNumber: number = 1,
    pageSize: number = 20,
    search: string = ''
  ) {
    this.getAll(pageNumber, pageSize, search);
  }

  protected getItemsFromResponse(
    response: TranslatorListResponse
  ): Translator[] | undefined {
    return response.translators;
  }

  protected getPaginationFromResponse(
    response: TranslatorListResponse
  ): TPaginationInfo | undefined {
    return response.pagination;
  }
}
