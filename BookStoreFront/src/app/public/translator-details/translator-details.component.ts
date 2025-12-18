import { Component, OnInit, inject, DestroyRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslatorPublicService } from '../../services/Public/translator-public.service';
import { ImageService } from '../../services/image.service';
import { AuthService } from '../../services/auth.service';
import { UserWishListService } from '../../services/user/user-wish-list.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-translator-details',
  standalone: false,
  templateUrl: './translator-details.component.html',
  styleUrl: './translator-details.component.css',
})
export class TranslatorDetailsComponent implements OnInit {
  private destroyRef = inject(DestroyRef);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  public translatorService = inject(TranslatorPublicService);
  public imageService = inject(ImageService);
  private authService = inject(AuthService);
  private wishlistService = inject(UserWishListService);

  translatorId!: number;
  isLoggedIn = false;

  ngOnInit(): void {
    // Translator still uses numeric ID
    this.route.paramMap
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((params) => {
        this.translatorId = Number(params.get('id'));
        this.loadTranslator();
      });

    this.authService.isLoggedIn$
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((isLogged) => {
        this.isLoggedIn = isLogged;
      });
  }

  loadTranslator(): void {
    this.translatorService.getTranslatorDetails(this.translatorId);
  }

  toggleWishlist(bookId: number, event: MouseEvent) {
    event.stopPropagation();
    this.wishlistService.ToggleWishlist(bookId);
  }

  goToBookDetails(id: number) {
    this.router.navigate(['/books', id]);
  }

  changePage(page: number) {
    const pagination = this.translatorService.details()?.books.pagination;
    if (!pagination || page === pagination.pageNumber) return;
    this.translatorService.getTranslatorDetails(this.translatorId, page);
  }

  getPageArray(): number[] {
    const pagination = this.translatorService.details()?.books.pagination;
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
