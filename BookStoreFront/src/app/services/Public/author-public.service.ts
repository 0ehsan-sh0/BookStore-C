import { HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthorDetails } from '../../models/author';
import { BasePublicService } from './base-public.service';
import { BPaginationInfo } from '../../models/book';

@Injectable({
  providedIn: 'root',
})
export class AuthorPublicService extends BasePublicService<
  AuthorDetails,
  any,
  any,
  BPaginationInfo
> {
  protected override readonly apiUrl = 'api/author';

  constructor() {
    super({
      pageNumber: 1,
      pageSize: 20,
      totalCount: 0,
      totalPages: 1,
    });
  }

  getAuthorDetails(
    authorId: number,
    pageNumber: number = 1,
    pageSize: number = 20
  ) {
    const params = new HttpParams()
      .set('PageNumber', pageNumber.toString())
      .set('PageSize', pageSize.toString());

    this.getDetails(authorId, params);
  }
}
