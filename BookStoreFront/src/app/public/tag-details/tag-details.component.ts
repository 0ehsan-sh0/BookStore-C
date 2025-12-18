import { Component, OnInit, inject, DestroyRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TagPublicService } from '../../services/Public/tag-public.service';
import { ImageService } from '../../services/image.service';
import { AuthService } from '../../services/auth.service';
import { UserWishListService } from '../../services/user/user-wish-list.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-tag-details',
  standalone: false,
  templateUrl: './tag-details.component.html',
  styleUrl: './tag-details.component.css',
})
export class TagDetailsComponent implements OnInit {
  private destroyRef = inject(DestroyRef);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  public tagService = inject(TagPublicService);
  public imageService = inject(ImageService);
  private authService = inject(AuthService);
  private wishlistService = inject(UserWishListService);

  tagUrl!: string;
  isLoggedIn = false;

  ngOnInit(): void {
    // Tag uses URL slug as ID
    this.route.paramMap
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((params) => {
        this.tagUrl = params.get('id') || '';
        this.loadTag();
      });

    this.authService.isLoggedIn$
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((isLogged) => {
        this.isLoggedIn = isLogged;
      });
  }

  loadTag(): void {
    this.tagService.getTagDetails(this.tagUrl);
  }

  toggleWishlist(bookId: number, event: MouseEvent) {
    event.stopPropagation();
    this.wishlistService.ToggleWishlist(bookId);
  }

  goToBookDetails(id: number) {
    this.router.navigate(['/books', id]);
  }

  changePage(page: number) {
    const pagination = this.tagService.details()?.books.pagination;
    if (!pagination || page === pagination.pageNumber) return;
    this.tagService.getTagDetails(this.tagUrl, page);
  }

  getPageArray(): number[] {
    const pagination = this.tagService.details()?.books.pagination;
    if (!pagination) return [];

    const total = pagination.totalPages;
    const current = pagination.pageNumber;

    const pages: number[] = [];

    for (let i = 1; i <= total; i++) {
      if (i === 1 || i === total || Math.abs(i - current) <= 1) {
        pages.push(i);
      } else if (i === current - 2 || i === current + 2) {
        pages.push(-1);
      }
    }

    return [...new Set(pages)];
  }
}
