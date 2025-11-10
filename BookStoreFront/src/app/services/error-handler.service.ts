import { Injectable } from '@angular/core';
import { AlertService } from '../ui-service/alert.service';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class ErrorHandlerService {
    constructor(private alertService: AlertService) {}

  handleError(error: HttpErrorResponse): string[] {
    const messages: string[] = [];

    if (!error) {
      const msg = 'خطای ناشناخته رخ داد';
      this.alertService.show(msg, 'error');
      return [msg];
    }

    let userMessage = 'خطای ناشناخته رخ داد';

    switch (error.status) {
      case 0:
        userMessage = 'اتصال به سرور برقرار نشد. لطفاً اتصال اینترنت را بررسی کنید.';
        break;
      case 400:
        userMessage = 'درخواست نامعتبر است.';
        break;
      case 401:
        userMessage = 'دسترسی غیرمجاز. لطفاً دوباره وارد شوید.';
        break;
      case 403:
        userMessage = 'شما مجوز انجام این عملیات را ندارید.';
        break;
      case 404:
        userMessage = 'مورد مورد نظر یافت نشد.';
        break;
      case 408:
        userMessage = 'زمان درخواست به پایان رسید. لطفاً مجدداً تلاش کنید.';
        break;
      case 500:
        userMessage = 'خطای داخلی سرور رخ داد.';
        break;
      case 503:
        userMessage = 'سرور در دسترس نیست. لطفاً بعداً تلاش کنید.';
        break;
      default:
        // Backend may return an object with a message or array of errors
        if (error.error) {
          if (typeof error.error === 'string') {
            userMessage = error.error;
          } else if (error.error.message) {
            userMessage = error.error.message;
          } else if (Array.isArray(error.error)) {
            // e.g. ["Error1", "Error2"]
            messages.push(...error.error);
          } else if (typeof error.error === 'object') {
            // collect possible validation messages
            for (const key in error.error) {
              if (Array.isArray(error.error[key])) {
                messages.push(...error.error[key]);
              } else if (typeof error.error[key] === 'string') {
                messages.push(error.error[key]);
              }
            }
          } else {
            userMessage = error.message || 'خطای ناشناخته رخ داد';
          }
        } else {
          userMessage = error.message || 'خطای ناشناخته رخ داد';
        }
        break;
    }

    // Always show at least one message to the user
    if (messages.length === 0) messages.push(userMessage);

    // Show the first or combined message in the alert service
    this.alertService.show(messages.join('\n'), 'error');

    // Return the list of messages for further handling
    return messages;
  }
}
