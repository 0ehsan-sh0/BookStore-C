import { Component, OnInit, inject, DestroyRef } from '@angular/core';
import { User } from '../../models/user';
import { AuthService } from '../../services/auth.service';
import { NgForm } from '@angular/forms';
import { UserPanelService } from '../../services/user/user-panel.service';
import { UserWishListService } from '../../services/user/user-wish-list.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-user-profile',
  standalone: false,
  templateUrl: './user-profile.component.html',
  styleUrl: './user-profile.component.css',
})
export class UserProfileComponent implements OnInit {
  private destroyRef = inject(DestroyRef);
  private authService = inject(AuthService);
  private userPanelService = inject(UserPanelService);
  private wishlistService = inject(UserWishListService);

  user: User = {
    name: '',
    lastName: '',
    mobile: '',
  } as User;

  // Counts
  ordersCount = 0;
  wishlistCount = 0;

  ngOnInit() {
    this.authService.user$
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((user) => {
        if (user) this.user = user;
      });

    this.userPanelService.invoicePagination
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((p) => (this.ordersCount = p.totalCount || 0));

    // wishlistService.pagination is now a signal
    this.wishlistCount = this.wishlistService.pagination().totalCount || 0;

    // Initial fetch if needed
    this.userPanelService
      .getUserInvoicesCount()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((res) => {
        this.ordersCount = res.data?.pagination?.totalCount || 0;
      });

    this.wishlistService.getUserWishlist(1, 1);
  }

  onSubmit(form: NgForm) {
    if (form.valid) {
      this.userPanelService.updateUser(form.value);
    }
  }
}
