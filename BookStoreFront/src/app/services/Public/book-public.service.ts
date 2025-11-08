import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AlertService } from '../../ui-service/alert.service';
import { ErrorHandlerService } from '../error-handler.service';
import { BehaviorSubject } from 'rxjs';
import { BookAllData, BookListResponse } from '../../models/book';
import { ApiResponse } from '../../models/apiResponse';

@Injectable({
  providedIn: 'root'
})
export class BookPublicService {
  private readonly apiUrl = 'api/book';

  constructor(
    private http: HttpClient,
    private alertService: AlertService,
    private errorHandler: ErrorHandlerService
  ) {}

  newBooks = new BehaviorSubject<BookAllData[]>([]);
  recommendedBooks = new BehaviorSubject<BookAllData[]>([]);

  getNewBooks(pageNumber: number = 1, pageSize: number = 20, isRecommended: boolean = false) {
    let params = new HttpParams()
      .set('PageNumber', pageNumber.toString());
    if (isRecommended) {
      params = params.set('IsRecommended', isRecommended.toString());
    } else {
      params = params.set('IsRecommended', 'false');
    }
    this.http.get<ApiResponse<BookListResponse>>(`${this.apiUrl}`, { params }).subscribe({
      next: (response) => {
        if (isRecommended) {
          this.recommendedBooks.next(response.data?.books || []);
        } else {
          this.newBooks.next(response.data?.books || []);
        }
      },
      error: (err) => {
        this.errorHandler.handleError(err);
      },
    });
  }
}
