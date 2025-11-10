import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  mobile: string = '';
  password: string = '';
  code: string = '';

  loading = false;
  errorMessage: string | null = null;
  showCodeLogin = false;
  isSendingCode = false;
  countDown = 0;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {
    
  }

  toggleLoginMethod(useCode: boolean) {
    this.showCodeLogin = useCode;
    this.errorMessage = null;
  }

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

    this.authService.sendCode(this.mobile);

    setTimeout(() => {
      this.isSendingCode = false;
      this.toggleLoginMethod(true);
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

    this.loading = true;
    this.errorMessage = null;

    const loginMethod = this.showCodeLogin ? 'code' : 'password';
    const password = loginMethod === 'password' ? this.password : '';
    const code = loginMethod === 'code' ? this.code : undefined;

    this.authService.login(this.mobile, password, code);

    this.authService.isLoggedIn$.subscribe(isLoggedIn => {
      this.loading = false;
      if (isLoggedIn) {
        this.router.navigate(['/']);
      }
    });
  }
}
