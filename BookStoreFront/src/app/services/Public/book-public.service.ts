import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AlertService } from '../../ui-service/alert.service';
import { ErrorHandlerService } from '../error-handler.service';
import { BehaviorSubject } from 'rxjs';
import { BookAllData } from '../../models/book';
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
  getNewBooks() {
    this.http
      .get<ApiResponse<BookAllData[]>>(`${this.apiUrl}/new`)
      .subscribe({
        next: (response) => {
          this.newBooks.next([...(response.data ?? [])]);
          console.log(response.data);
          
          
        },
        error: (err) => {
          this.errorHandler.handleError(err);
        },
      });
  }
}
