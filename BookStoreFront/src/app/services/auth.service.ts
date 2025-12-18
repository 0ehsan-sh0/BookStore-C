import {
  Injectable,
  signal,
  computed,
  inject,
  DestroyRef,
} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { AlertService } from '../ui-service/alert.service';
import { ErrorHandlerService } from './error-handler.service';
import { toObservable, takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { tap } from 'rxjs/operators';
import {
  LoginRequest,
  RegisterRequest,
  LoginResponse,
  RegisterResponse,
  SendCodeRequest,
  LogoutResponse,
} from '../models/auth';
import { ApiResponse } from '../models/apiResponse';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly apiUrl = 'api/auth';
  private http = inject(HttpClient);
  private alertService = inject(AlertService);
  private errorHandler = inject(ErrorHandlerService);
  private router = inject(Router);
  private destroyRef = inject(DestroyRef);

  private userSig = signal<User | null>(null);
  user$ = toObservable(this.userSig);
  user = this.userSig.asReadonly();

  private isLoggedInSig = signal<boolean>(false);
  isLoggedIn$ = toObservable(this.isLoggedInSig);
  isLoggedIn = this.isLoggedInSig.asReadonly();

  loginErrors: string[] = [];
  registerErrors: string[] = [];

  constructor() {}

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
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (response) => {
          this.isLoggedInSig.set(true);
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
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (response) => {
          this.isLoggedInSig.set(true);
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
      .pipe(takeUntilDestroyed(this.destroyRef))
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
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: () => {
          this.isLoggedInSig.set(false);
          this.userSig.set(null);
          document.cookie =
            'access_token=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;';
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
          this.userSig.set(response.data ?? null);
          this.isLoggedInSig.set(!!response.data);
        },
        error: (err) => {
          this.userSig.set(null);
          this.isLoggedInSig.set(false);
        },
      })
    );
  }
}
