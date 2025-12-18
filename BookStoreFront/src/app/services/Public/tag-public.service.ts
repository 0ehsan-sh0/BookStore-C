import { HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TagDetails } from '../../models/tag';
import { BasePublicService } from './base-public.service';

@Injectable({
  providedIn: 'root',
})
export class TagPublicService extends BasePublicService<
  TagDetails,
  any,
  any,
  any
> {
  protected override readonly apiUrl = 'api/tag';

  constructor() {
    super(null);
  }

  getTagDetails(tagUrl: string, pageNumber: number = 1, pageSize: number = 20) {
    const params = new HttpParams()
      .set('PageNumber', pageNumber.toString())
      .set('PageSize', pageSize.toString());

    this.getDetails(tagUrl, params);
  }
}
