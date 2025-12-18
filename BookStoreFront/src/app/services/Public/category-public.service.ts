import { HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Category, CategoryDetails } from '../../models/category';
import { BasePublicService } from './base-public.service';
import { ApiResponse } from '../../models/apiResponse';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Injectable({
  providedIn: 'root',
})
export class CategoryPublicService extends BasePublicService<
  CategoryDetails,
  Category,
  Category[],
  any
> {
  protected override readonly apiUrl = 'api/category';

  constructor() {
    super(null);
  }

  getCategoriesWithSub() {
    this.http
      .get<ApiResponse<Category[]>>(`${this.apiUrl}`)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (response) => {
          this.itemsSig.set(response.data ?? []);
        },
        error: (err) => {
          this.errorHandler.handleError(err);
        },
      });
  }

  getCategoryDetails(
    categoryUrl: string,
    pageNumber: number = 1,
    pageSize: number = 20
  ) {
    const params = new HttpParams()
      .set('PageNumber', pageNumber.toString())
      .set('PageSize', pageSize.toString());

    this.getDetails(categoryUrl, params);
  }
}
