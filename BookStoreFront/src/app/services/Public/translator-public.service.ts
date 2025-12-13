import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AlertService } from '../../ui-service/alert.service'; // Assuming this exists based on other services
import { ErrorHandlerService } from '../error-handler.service';
import { BehaviorSubject } from 'rxjs';
import { TranslatorDetails } from '../../models/translator';
import { ApiResponse } from '../../models/apiResponse';

@Injectable({
  providedIn: 'root',
})
export class TranslatorPublicService {
  private readonly apiUrl = 'api/translator';

  constructor(
    private http: HttpClient,
    private alertService: AlertService,
    private errorHandler: ErrorHandlerService
  ) {}

  translatorDetails = new BehaviorSubject<TranslatorDetails | null>(null);

  getTranslatorDetails(
    translatorId: number,
    pageNumber: number = 1,
    pageSize: number = 20
  ) {
    const params = new HttpParams()
      .set('PageNumber', pageNumber.toString())
      .set('PageSize', pageSize.toString());

    this.http
      .get<ApiResponse<TranslatorDetails>>(`${this.apiUrl}/${translatorId}`, {
        params,
      })
      .subscribe({
        next: (response) => {
          this.translatorDetails.next(response.data ?? null);
        },
        error: (err) => {
          this.errorHandler.handleError(err);
        },
      });
  }
}
