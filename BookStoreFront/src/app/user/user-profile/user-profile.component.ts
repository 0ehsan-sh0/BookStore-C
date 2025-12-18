import { Component } from '@angular/core';
import { User } from '../../models/user';
import { AuthService } from '../../services/auth.service';
import { NgForm } from '@angular/forms';
import { UserPanelService } from '../../services/user/user-panel.service';
import { UserWishListService } from '../../services/user/user-wish-list.service';

@Component({
  selector: 'app-user-profile',
  standalone: false,
  templateUrl: './user-profile.component.html',
  styleUrl: './user-profile.component.css',
})
export class UserProfileComponent {
  user: User = {
    name: '',
    lastName: '',
    mobile: '',
    // add any other fields your User model requires
  } as User;

  // Counts
  ordersCount = 0;
  wishlistCount = 0;

  constructor(
    private authService: AuthService,
    private userPanelService: UserPanelService,
    private wishlistService: UserWishListService
  ) {}

  ngOnInit() {
    this.authService.user.subscribe((user) => {
      if (user) this.user = user;
    });

    // Fetch counts
    // We can rely on the subscriptions if the data is already fetched (e.g. by Sidebar),
    // but to be safe and ensure data is fresh if user lands here directly:
    // Actually, sidebar is always there, so it fetches initial data.
    // But we should subscribe to the subjects.

    this.userPanelService.invoicePagination.subscribe(
      (p) => (this.ordersCount = p.totalCount || 0)
    );
    this.wishlistService.pagination.subscribe(
      (p) => (this.wishlistCount = p?.totalCount || 0)
    );

    // Initial fetch if needed (optional if sidebar handles it, but good for standalone correctness)
    this.userPanelService.getUserInvoicesCount().subscribe((res) => {
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
