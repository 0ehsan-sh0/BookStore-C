import { Component, OnInit, inject, DestroyRef } from '@angular/core';
import { Router } from '@angular/router';
import { ThemeControllerService } from '../../ui-service/theme-controller.service';
import { AuthService } from '../../services/auth.service';
import { UserCartService } from '../../services/user/user-cart.service';
import { User, UserRole } from '../../models/user';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-public-header',
  standalone: false,
  templateUrl: './public-header.component.html',
  styleUrl: './public-header.component.css',
})
export class PublicHeaderComponent implements OnInit {
  private destroyRef = inject(DestroyRef);
  private themeController = inject(ThemeControllerService);
  public authService = inject(AuthService);
  private router = inject(Router);
  public userCartService = inject(UserCartService);

  public isLight = true;
  UserRole = UserRole; // Expose UserRole enum to the template

  toggleTheme() {
    this.themeController.toggleTheme();
    this.isLight = !this.isLight;
  }

  ngOnInit(): void {
    this.isLight = this.themeController.theme.getValue() === 'cupcake';
  }

  navigateToAuth() {
    if (this.authService.isLoggedIn()) {
      // Implement logout - the new service handles everything internally
      this.authService.logout();
    } else {
      // Navigate to login
      this.router.navigate(['/login']);
    }
  }
}
