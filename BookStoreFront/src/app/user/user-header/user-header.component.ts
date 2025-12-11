import { Component } from '@angular/core';
import { ThemeControllerService } from '../../ui-service/theme-controller.service';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { UserCartService } from '../../services/user/user-cart.service';
import { User } from '../../models/user';

@Component({
  selector: 'app-user-header',
  standalone: false,
  templateUrl: './user-header.component.html',
  styleUrl: './user-header.component.css',
})
export class UserHeaderComponent {
  public isLight = true;
  isLoggedIn = false;
  cartItemCount = 0;
  private cartSubscription: Subscription | undefined; // To manage subscription

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
