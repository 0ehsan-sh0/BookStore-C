import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ThemeControllerService } from '../../ui-service/theme-controller.service';
import { AuthService } from '../../services/auth.service';
import { UserCartService } from '../../services/user/user-cart.service';
import { Subscription } from 'rxjs';
import { User } from '../../models/user';
import { UserRole } from '../../models/user';

@Component({
  selector: 'app-public-header',
  standalone: false,
  templateUrl: './public-header.component.html',
  styleUrl: './public-header.component.css',
})
export class PublicHeaderComponent {
  public isLight = true;
  isLoggedIn = false;
  cartItemCount = 0;
  private cartSubscription: Subscription | undefined; // To manage subscription
  user : User | null = null;
  UserRole = UserRole; // Expose UserRole enum to the template

  constructor(
    private themeController: ThemeControllerService,
    private authService: AuthService,
    private router: Router,
    private userCartService: UserCartService
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

    // Get user info
    this.authService.user.subscribe((user) => {
      this.user = user;
    });

    // Subscribe to the cart item count observable
    this.cartSubscription = this.userCartService.itemCount$.subscribe(
      (count) => {
        this.cartItemCount = count;
      }
    );
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

  ngOnDestroy(): void {
    // Unsubscribe to prevent memory leaks when the component is destroyed
    if (this.cartSubscription) {
      this.cartSubscription.unsubscribe();
    }
  }
}
