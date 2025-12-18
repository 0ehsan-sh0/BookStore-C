import { Component } from '@angular/core';
import { User } from '../../models/user';
import { AuthService } from '../../services/auth.service';
import { UserPanelService } from '../../services/user/user-panel.service';
import { UserCommentService } from '../../services/user/user-comment.service';
import { UserWishListService } from '../../services/user/user-wish-list.service';

@Component({
  selector: 'app-sidebar-user',
  standalone: false,
  templateUrl: './sidebar-user.component.html',
  styleUrl: './sidebar-user.component.css',
})
export class SidebarUserComponent {
  user: User = {
    name: '',
    lastName: '',
    mobile: '',
    // add any other fields your User model requires
  } as User;

  // Counts
  ordersCount = 0;
  wishlistCount = 0;
  commentsCount = 0;
  // addressesCount = 0; // Not strictly requested as badge, but good to have if needed (user said "for others can you calculate"). But UI usually shows badges for Orders/Wishlist/Comments. Template has no badge for addresses currently, but I'll fetch it just in case or if requested. Actually the user said "for profile no need ... for others can you calculate". The template shows badge for Orders, Wishlist, Comments. I will implement those.

  constructor(
    private authService: AuthService,
    private userPanelService: UserPanelService, // For Orders
    private wishlistService: UserWishListService,
    private commentService: UserCommentService
  ) {
    this.authService.initUser();
  }

  ngOnInit() {
    this.authService.user.subscribe((user) => {
      if (user) {
        this.user = user;
        this.loadCounts();
      }
    });

    // Subscribe to updates
    this.userPanelService.invoicePagination.subscribe(
      (p) => (this.ordersCount = p.totalCount || 0)
    );
    this.wishlistService.pagination.subscribe(
      (p) => (this.wishlistCount = p?.totalCount || 0)
    );
    this.commentService.pagination.subscribe(
      (p) => (this.commentsCount = p.totalCount || 0)
    );
  }

  loadCounts() {
    this.userPanelService.getUserInvoicesCount().subscribe((res) => {
      this.ordersCount = res.data?.pagination?.totalCount || 0;
    });
    this.wishlistService.getUserWishlist(1, 1);
    this.commentService.getUserComments(1, 1);
    // Remove duplicate call
    // this.commentService.getUserComments(1, 1);
  }

  // in the component that contains the drawer template (e.g. user-public.component.ts or header)
  closeUserSidebar(): void {
    const el = document.getElementById(
      'user-sidebar'
    ) as HTMLInputElement | null;
    if (el) {
      el.checked = false; // closes the drawer
    }
  }
}
