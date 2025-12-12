import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AlertService } from '../../ui-service/alert.service';
import { ErrorHandlerService } from '../error-handler.service';
import { BehaviorSubject } from 'rxjs';
import { AuthorDetails } from '../../models/author';
import { ApiResponse } from '../../models/apiResponse';

@Injectable({
  providedIn: 'root',
})
export class AuthorPublicService {
  private readonly apiUrl = 'api/author';

  constructor(
    private http: HttpClient,
    private alertService: AlertService,
    private errorHandler: ErrorHandlerService
  ) {}

  authorDetails = new BehaviorSubject<AuthorDetails | null>(null);

  getAuthorDetails(
    authorId: number,
    pageNumber: number = 1,
    pageSize: number = 20
  ) {
    const params = new HttpParams()
      .set('PageNumber', pageNumber.toString())
      .set('PageSize', pageSize.toString());

    this.http
      .get<ApiResponse<AuthorDetails>>(`${this.apiUrl}/${authorId}`, { params })
      .subscribe({
        next: (response) => {
          this.authorDetails.next(response.data ?? null);
        },
        error: (err) => {
          this.authorDetails.next(null);
          this.errorHandler.handleError(err);
        },
      });
  }
}
