import { Component, OnInit, inject, DestroyRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CategoryPublicService } from '../../services/Public/category-public.service';
import { ImageService } from '../../services/image.service';
import { AuthService } from '../../services/auth.service';
import { UserWishListService } from '../../services/user/user-wish-list.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-category-details',
  standalone: false,
  templateUrl: './category-details.component.html',
  styleUrl: './category-details.component.css',
})
export class CategoryDetailsComponent implements OnInit {
  private destroyRef = inject(DestroyRef);
  categoryUrl!: string;
  isLoggedIn = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    public categoryService: CategoryPublicService,
    public imageService: ImageService,
    private authService: AuthService,
    private wishlistService: UserWishListService
  ) {}

  ngOnInit(): void {
    // Category uses URL slug as ID
    this.route.paramMap
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((params) => {
        this.categoryUrl = params.get('id') || '';
        this.loadCategory();
      });

    this.authService.isLoggedIn$
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((isLogged) => {
        this.isLoggedIn = isLogged;
      });
  }

  loadCategory(): void {
    this.categoryService.getCategoryDetails(this.categoryUrl);
  }

  toggleWishlist(bookId: number, event: MouseEvent) {
    event.stopPropagation();
    this.wishlistService.ToggleWishlist(bookId);
  }

  goToBookDetails(id: number) {
    this.router.navigate(['/books', id]);
  }

  changePage(page: number) {
    const pagination = this.categoryService.details()?.books.pagination;
    if (!pagination || page === pagination.pageNumber) return;
    this.categoryService.getCategoryDetails(this.categoryUrl, page);
  }

  getPageArray(): number[] {
    const pagination = this.categoryService.details()?.books.pagination;
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
