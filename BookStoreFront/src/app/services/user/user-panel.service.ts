import { Injectable } from '@angular/core';
import { User } from '../../models/user';
import { ApiResponse } from '../../models/apiResponse';
import { HttpClient } from '@angular/common/http';
import { AlertService } from '../../ui-service/alert.service';
import { ErrorHandlerService } from '../error-handler.service';
import { BehaviorSubject } from 'rxjs';
import { AuthService } from '../auth.service';

@Injectable({
  providedIn: 'root',
})
export class UserPanelService {
  private readonly apiUrl = 'api/user';

  constructor(
    private http: HttpClient,
    private alertService: AlertService,
    private errorHandler: ErrorHandlerService,
  ) {}

  user = new BehaviorSubject<User | null>(null);

  updateUser(user: User) {
    this.http.put<ApiResponse<User>>(`${this.apiUrl}`, user).subscribe({
      next: (response) => {
        // The access token will be stored in a cookie by the backend
        this.user.next(response.data as User);
        // Update localStorage
        localStorage.setItem('user', JSON.stringify(response.data as User));
        this.alertService.show('اطلاعات شما با موفقیت به روز شد', 'success');
      },
      error: (err) => {
        this.errorHandler.handleError(err);
      },
    });
  }
}
