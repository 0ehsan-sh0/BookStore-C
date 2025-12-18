import { Component, inject, DestroyRef } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-register',
  standalone: false,
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent {
  private destroyRef = inject(DestroyRef);
  private authService = inject(AuthService);
  private router = inject(Router);

  mobile: string = '';
  password: string = '';
  confirmPassword: string = '';
  code: string = '';

  loading = false;
  errorMessage: string | null = null;
  successMessage: string | null = null;
  isSendingCode = false;
  countDown = 0;

  sendCode() {
    if (!this.mobile) {
      this.errorMessage = 'لطفا شماره موبایل را وارد کنید';
      return;
    }

    if (!/^09\d{9}$/.test(this.mobile)) {
      this.errorMessage = 'شماره موبایل معتبر نیست';
      return;
    }

    this.isSendingCode = true;
    this.errorMessage = null;

    this.authService.sendCode(this.mobile, true);

    setTimeout(() => {
      this.isSendingCode = false;
      this.startCountDown();
    }, 1000);
  }

  startCountDown() {
    this.countDown = 60;
    const timer = setInterval(() => {
      this.countDown--;
      if (this.countDown <= 0) clearInterval(timer);
    }, 1000);
  }

  onSubmit(form: any) {
    if (!form.valid) {
      this.errorMessage = 'لطفا فیلدها را به درستی پر کنید';
      return;
    }

    if (this.password !== this.confirmPassword) {
      this.errorMessage = 'رمز عبور و تکرار آن مطابقت ندارند';
      return;
    }

    this.loading = true;
    this.errorMessage = null;
    this.successMessage = null;

    this.authService.register(this.mobile, this.password, this.code);

    this.authService.isLoggedIn$
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((isLoggedIn) => {
        this.loading = false;
        if (isLoggedIn) {
          this.successMessage = 'ثبت‌نام با موفقیت انجام شد';
          setTimeout(() => this.router.navigate(['/']), 2000);
        }
      });
  }
}
