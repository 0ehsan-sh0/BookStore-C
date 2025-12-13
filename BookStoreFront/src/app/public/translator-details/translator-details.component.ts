import { Component, OnInit } from '@angular/core';
import { TranslatorDetails } from '../../models/translator';
import { BookAllData, BPaginationInfo } from '../../models/book';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslatorPublicService } from '../../services/Public/translator-public.service';
import { ImageService } from '../../services/image.service';
import { AuthService } from '../../services/auth.service';
import { UserWishListService } from '../../services/user/user-wish-list.service';

@Component({
  selector: 'app-translator-details',
  standalone: false,
  templateUrl: './translator-details.component.html',
  styleUrl: './translator-details.component.css',
})
export class TranslatorDetailsComponent implements OnInit {
  translatorId!: number;

  translator: TranslatorDetails = {} as TranslatorDetails;
  books: BookAllData[] = [];
  filteredBooks: BookAllData[] = [];

  pagination: BPaginationInfo = {
    pageNumber: 1,
    pageSize: 20,
    totalCount: 0,
    totalPages: 1,
  };

  isLoggedIn = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private translatorService: TranslatorPublicService,
    public imageService: ImageService,
    private authService: AuthService,
    private wishlistService: UserWishListService
  ) {}

  ngOnInit(): void {
    // Translator still uses numeric ID
    this.route.paramMap.subscribe((params) => {
      this.translatorId = Number(params.get('id'));
      this.loadTranslator();
    });

    this.authService.isLoggedIn$.subscribe((isLogged) => {
      this.isLoggedIn = isLogged;
    });
  }

  loadTranslator(): void {
    this.translatorService.getTranslatorDetails(
      this.translatorId,
      this.pagination.pageNumber,
      this.pagination.pageSize
    );

    this.translatorService.translatorDetails.subscribe((details) => {
      if (!details?.translator) return;

      this.translator = details;
      this.books = details.books.books ?? [];
      this.pagination = details.books.pagination ?? this.pagination;

      this.filteredBooks = [...this.books];
    });
  }

  toggleWishlist(bookId: number, event: MouseEvent) {
    event.stopPropagation();
    this.wishlistService.ToggleWishlist(bookId);
  }

  goToBookDetails(id: number) {
    this.router.navigate(['/books', id]);
  }

  changePage(page: number) {
    if (page === this.pagination.pageNumber) return;
    this.translatorService.getTranslatorDetails(
      this.translatorId,
      page,
      this.pagination.pageSize
    );
  }

  getPageArray(): number[] {
    const total = this.pagination.totalPages;
    const current = this.pagination.pageNumber;

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
