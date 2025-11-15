import { Component, ViewChild, ViewContainerRef } from '@angular/core';
import { ThemeControllerService } from './ui-service/theme-controller.service';
import { AlertService } from './ui-service/alert.service';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrl: './app.component.css',
})
export class AppComponent {
  @ViewChild('alertContainer', { read: ViewContainerRef })
  vcRef!: ViewContainerRef;
  title = 'BookStoreFront';
  theme = 'cupcake';
  constructor(
    private alertService: AlertService,
    private themeController: ThemeControllerService,
    private authService: AuthService
  ) {}
  ngOnInit() {
    this.themeController.theme.subscribe((theme) => {
      this.theme = theme;
      document.documentElement.setAttribute('data-theme', theme);
    });

    this.authService.initUser();
  }

  ngAfterViewInit() {
    this.alertService.registerContainer(this.vcRef);
  }
}
