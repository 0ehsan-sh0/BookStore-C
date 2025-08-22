import { Injectable } from '@angular/core';
import { AlertService } from '../ui-service/alert.service';
import { HttpErrorResponse } from '@angular/common/http';
import { ApiResponse } from '../models/apiResponse';

@Injectable({
  providedIn: 'root',
})
export class ErrorHandlerService {
  constructor(private alertService: AlertService) {}

  handleError(error: HttpErrorResponse): string[] {
    const apiError = error.error as ApiResponse<null>;

    if (apiError) {
      this.alertService.show(apiError.message || 'خطا رخ داد', 'error');

      if (apiError.errors) {
        const flattenedErrors: string[] = [];
        for (const key in apiError.errors) {
          if (apiError.errors.hasOwnProperty(key)) {
            flattenedErrors.push(...apiError.errors[key]);
          }
        }
        return flattenedErrors;
      } else {
        return [];
      }
    } else {
      this.alertService.show('خطای ناشناخته رخ داد', 'error');
      return [];
    }
  }
}
