import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ThemeControllerService } from '../../ui-service/theme-controller.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-public-header',
  standalone: false,
  templateUrl: './public-header.component.html',
  styleUrl: './public-header.component.css',
})
export class PublicHeaderComponent {
  public isLight = true;
  isLoggedIn = false;

  constructor(
    private themeController: ThemeControllerService,
    private authService: AuthService,
    private router: Router
  ) {}

  toggleTheme() {
    this.themeController.toggleTheme();
    this.isLight = !this.isLight;
  }

  ngOnInit(): void {
    this.themeController.theme.getValue() === 'cupcake'
      ? (this.isLight = true)
      : (this.isLight = false);

    // Check authentication status
    this.authService.isLoggedIn$.subscribe((isLoggedIn) => {
      this.isLoggedIn = isLoggedIn;
    });
  }

  navigateToAuth() {
    if (this.isLoggedIn) {
      // Implement logout - the new service handles everything internally
      this.authService.logout();
    } else {
      // Navigate to login
      this.router.navigate(['/login']);
    }
  }
}
