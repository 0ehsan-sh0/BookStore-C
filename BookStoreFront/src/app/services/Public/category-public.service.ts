import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AlertService } from '../../ui-service/alert.service';
import { ErrorHandlerService } from '../error-handler.service';
import { Category } from '../../models/category';
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
}
