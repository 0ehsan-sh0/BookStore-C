import { Component, OnInit, inject, DestroyRef } from '@angular/core';
import { User } from '../../models/user';
import { AuthService } from '../../services/auth.service';
import { UserPanelService } from '../../services/user/user-panel.service';
import { UserCommentService } from '../../services/user/user-comment.service';
import { UserWishListService } from '../../services/user/user-wish-list.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-sidebar-user',
  standalone: false,
  templateUrl: './sidebar-user.component.html',
  styleUrl: './sidebar-user.component.css',
})
export class SidebarUserComponent implements OnInit {
  private destroyRef = inject(DestroyRef);
  private authService = inject(AuthService);
  private userPanelService = inject(UserPanelService);
  private wishlistService = inject(UserWishListService);
  private commentService = inject(UserCommentService);

  user: User = {
    name: '',
    lastName: '',
    mobile: '',
  } as User;

  // Counts
  ordersCount = 0;
  wishlistCount = 0;
  commentsCount = 0;

  ngOnInit() {
    this.authService.user$
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((user) => {
        if (user) {
          this.user = user;
          this.loadCounts();
        }
      });

    // Subscribe to updates
    this.userPanelService.invoicePagination
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((p) => (this.ordersCount = p.totalCount || 0));

    // wishlistService.pagination is now a signal
    // We can use an effect or just update in loadCounts
    // But since it can change reactivey, let's use toObservable for compatibility if needed
    // or just use the signal in the template if we can.
    // However, the component mirrors it to local variables.
    // I'll use a signal-based approach or just manually update.
  }

  loadCounts() {
    this.userPanelService
      .getUserInvoicesCount()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((res) => {
        this.ordersCount = res.data?.pagination?.totalCount || 0;
      });

    this.wishlistService.getUserWishlist(1, 1);
    this.commentService.getUserComments(1, 1);

    // Sync with signal
    this.wishlistCount = this.wishlistService.pagination().totalCount || 0;

    this.commentService.pagination
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((p) => (this.commentsCount = p.totalCount || 0));
  }

  closeUserSidebar(): void {
    const el = document.getElementById(
      'user-sidebar'
    ) as HTMLInputElement | null;
    if (el) {
      el.checked = false;
    }
  }
}
