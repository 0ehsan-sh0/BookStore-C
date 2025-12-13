import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AlertService } from '../../ui-service/alert.service'; // Assuming this exists based on other services
import { ErrorHandlerService } from '../error-handler.service';
import { BehaviorSubject } from 'rxjs';
import { TagDetails } from '../../models/tag';
import { ApiResponse } from '../../models/apiResponse';

@Injectable({
  providedIn: 'root',
})
export class TagPublicService {
  private readonly apiUrl = 'api/tag';

  constructor(
    private http: HttpClient,
    private alertService: AlertService,
    private errorHandler: ErrorHandlerService
  ) {}

  tagDetails = new BehaviorSubject<TagDetails | null>(null);

  getTagDetails(tagUrl: string, pageNumber: number = 1, pageSize: number = 20) {
    const params = new HttpParams()
      .set('PageNumber', pageNumber.toString())
      .set('PageSize', pageSize.toString());

    this.http
      .get<ApiResponse<TagDetails>>(`${this.apiUrl}/${tagUrl}`, { params })
      .subscribe({
        next: (response) => {
          this.tagDetails.next(response.data ?? null);
        },
        error: (err) => {
          this.errorHandler.handleError(err);
        },
      });
  }
}
