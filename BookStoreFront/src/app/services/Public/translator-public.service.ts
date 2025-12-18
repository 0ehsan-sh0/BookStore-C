import { HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslatorDetails } from '../../models/translator';
import { BasePublicService } from './base-public.service';

@Injectable({
  providedIn: 'root',
})
export class TranslatorPublicService extends BasePublicService<
  TranslatorDetails,
  any,
  any,
  any
> {
  protected override readonly apiUrl = 'api/translator';

  constructor() {
    super(null);
  }

  getTranslatorDetails(
    translatorId: number,
    pageNumber: number = 1,
    pageSize: number = 20
  ) {
    const params = new HttpParams()
      .set('PageNumber', pageNumber.toString())
      .set('PageSize', pageSize.toString());

    this.getDetails(translatorId, params);
  }
}
