import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AlertService } from '../../ui-service/alert.service';
import { ErrorHandlerService } from '../error-handler.service';
import { Category, CategoryDetails } from '../../models/category';
import { BehaviorSubject } from 'rxjs';
import { ApiResponse } from '../../models/apiResponse';

@Injectable({
  providedIn: 'root',
})
export class CategoryPublicService {
  private readonly apiUrl = 'api/category';

  constructor(
    private http: HttpClient,
    private alertService: AlertService,
    private errorHandler: ErrorHandlerService
  ) {}

  categories = new BehaviorSubject<Category[]>([]);
  categoryDetails = new BehaviorSubject<CategoryDetails | null>(null);

  getCategoriesWithSub() {
    this.http
      .get<ApiResponse<Category[]>>(`${this.apiUrl}`)
      .subscribe({
        next: (response) => {
          this.categories.next(response.data ?? []);
        },
        error: (err) => {
          this.errorHandler.handleError(err);
        },
      });
  }

  getCategoryDetails(categoryUrl: string, pageNumber: number = 1, pageSize: number = 20) {
    const params = new HttpParams()
      .set('PageNumber', pageNumber.toString())
      .set('PageSize', pageSize.toString());
      
    this.http
      .get<ApiResponse<CategoryDetails>>(`${this.apiUrl}/${categoryUrl}`, { params })
      .subscribe({
        next: (response) => {
          this.categoryDetails.next(response.data ?? null);
        },
        error: (err) => {
          this.errorHandler.handleError(err);
        },
      });
  }
}
