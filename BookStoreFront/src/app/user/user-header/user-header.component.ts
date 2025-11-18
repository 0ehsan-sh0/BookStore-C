import { Component } from '@angular/core';
import { ThemeControllerService } from '../../ui-service/theme-controller.service';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-header',
  standalone: false,
  templateUrl: './user-header.component.html',
  styleUrl: './user-header.component.css'
})
export class UserHeaderComponent {
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
