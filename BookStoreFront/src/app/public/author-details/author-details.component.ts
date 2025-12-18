import { Component, DestroyRef, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthorPublicService } from '../../services/Public/author-public.service';
import { ImageService } from '../../services/image.service';
import { AuthService } from '../../services/auth.service';
import { UserWishListService } from '../../services/user/user-wish-list.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-author-details',
  standalone: false,
  templateUrl: './author-details.component.html',
  styleUrl: './author-details.component.css',
})
export class AuthorDetailsComponent implements OnInit {
  private destroyRef = inject(DestroyRef);
  authorId!: number;
  isLoggedIn = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    public authorService: AuthorPublicService,
    public imageService: ImageService,
    private authService: AuthService,
    private wishlistService: UserWishListService
  ) {}

  ngOnInit(): void {
    this.route.paramMap
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((params) => {
        this.authorId = Number(params.get('id'));
        this.loadAuthor();
      });

    this.authService.isLoggedIn$
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((isLogged) => {
        this.isLoggedIn = isLogged;
      });
  }

  loadAuthor(): void {
    this.authorService.getAuthorDetails(this.authorId);
  }

  // -----------------------------
  // UI Actions
  // -----------------------------

  toggleWishlist(bookId: number, event: MouseEvent) {
    event.stopPropagation();
    this.wishlistService.ToggleWishlist(bookId);
  }

  goToBookDetails(id: number) {
    this.router.navigate(['/books', id]);
  }

  changePage(page: number) {
    const pagination = this.authorService.details()?.books.pagination;
    if (!pagination || page === pagination.pageNumber) return;
    this.authorService.getAuthorDetails(this.authorId, page);
  }

  getPageArray(): number[] {
    const pagination = this.authorService.details()?.books.pagination;
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
