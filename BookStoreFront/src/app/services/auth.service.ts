import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { AlertService } from '../ui-service/alert.service';
import { ErrorHandlerService } from './error-handler.service';
import { BehaviorSubject, Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import {
  LoginRequest,
  RegisterRequest,
  LoginResponse,
  RegisterResponse,
  SendCodeRequest,
  LogoutResponse,
  MeResponse,
} from '../models/auth';
import { ApiResponse } from '../models/apiResponse';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly apiUrl = 'api/auth';

  constructor(
    private http: HttpClient,
    private alertService: AlertService,
    private errorHandler: ErrorHandlerService,
    private router: Router
  ) {}

  user = new BehaviorSubject<User | null>(null);
  loginResponse = new BehaviorSubject<LoginResponse | null>(null);
  isLoggedIn$ = new BehaviorSubject<boolean>(false);
  loginErrors: string[] = [];
  registerErrors: string[] = [];

  login(mobile: string, password: string, code?: string) {
    this.checkAuthStatus().subscribe(() => {
      this.router.navigate(['/']);
    });
    const request: LoginRequest = {
      mobile,
      password,
      code: code || undefined,
    };

    this.http
      .post<ApiResponse<LoginResponse>>(`${this.apiUrl}/login`, request, {
        withCredentials: true,
      })
      .subscribe({
        next: (response) => {
          // The access token will be stored in a cookie by the backend
          this.loginResponse.next(response.data!);
          this.isLoggedIn$.next(true);
          this.loginErrors = [];
          this.router.navigate(['/']);
          this.alertService.show('شما با موفقیت وارد شدید.', 'success');
        },
        error: (err) => {
          this.loginErrors = this.errorHandler.handleError(err);
        },
      });
  }

  register(mobile: string, password: string, code: string) {
    const request: RegisterRequest = {
      mobile,
      password,
      code,
    };

    this.http
      .post<ApiResponse<RegisterResponse>>(`${this.apiUrl}/register`, request, {
        withCredentials: true,
      })
      .subscribe({
        next: (response) => {
          // The access token will be stored in a cookie by the backend
          this.loginResponse.next(response.data as LoginResponse); // Cast to LoginResponse since they share similar structure
          this.isLoggedIn$.next(true);
          this.registerErrors = [];
          this.router.navigate(['/']);
          this.alertService.show('ثبت نام با موفقیت انجام شد', 'success');
        },
        error: (err) => {
          this.registerErrors = this.errorHandler.handleError(err);
        },
      });
  }

  sendCode(mobile: string, isRegister: boolean) {
    const request: SendCodeRequest = { mobile, isRegister };

    this.http
      .post<ApiResponse<any>>(`${this.apiUrl}/send-code`, request)
      .subscribe({
        next: () => {
          this.alertService.show('کد تأیید ارسال شد', 'success');
        },
        error: (err) => {
          this.errorHandler.handleError(err);
        },
      });
  }

  logout() {
    this.http
      .post<ApiResponse<LogoutResponse>>(`${this.apiUrl}/logout`, {}, {})
      .subscribe({
        next: () => {
          // Clear user data
          this.loginResponse.next(null);
          this.isLoggedIn$.next(false);
          this.user.next(null);
          // Remove the access token cookie
          document.cookie =
            'access_token=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;';
          // Navigate to home page
          this.router.navigate(['/']);
          this.alertService.show('شما با موفقیت خارج شدید.', 'success');
        },
        error: (err) => {
          this.errorHandler.handleError(err);
        },
      });
  }

  initUser() {
    this.checkAuthStatus().subscribe();
  }

  checkAuthStatus() {
    return this.http.get<ApiResponse<User>>(`${this.apiUrl}/me`).pipe(
      tap({
        next: (response) => {
          this.user.next(response.data!);
          this.isLoggedIn$.next(true);
        },
        error: (err) => {
          this.user.next(null);
          this.isLoggedIn$.next(false);
        },
      })
    );
  }
}
