import {
  Component,
  OnInit,
  inject,
  DestroyRef,
  signal,
  computed,
} from '@angular/core';
import { BookAllData, BPaginationInfo } from '../../models/book';
import { BookPublicService } from '../../services/Public/book-public.service';
import { ImageService } from '../../services/image.service';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { UserWishListService } from '../../services/user/user-wish-list.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-book',
  standalone: false,
  templateUrl: './book.component.html',
  styleUrl: './book.component.css',
})
export class BookComponent implements OnInit {
  private destroyRef = inject(DestroyRef);
  isLoggedIn = false;

  searchTerm = signal<string>('');
  sortOption = signal<string>('newest');

  // Computed signal for filtered and sorted books
  filteredBooks = computed(() => {
    const books = this.bookService.items();
    const term = this.searchTerm().toLowerCase();
    const sort = this.sortOption();

    let result = books.filter(
      (b) =>
        b.name.toLowerCase().includes(term) ||
        b.author?.name?.toLowerCase().includes(term)
    );

    switch (sort) {
      case 'priceLow':
        result.sort((a, b) => a.price - b.price);
        break;
      case 'priceHigh':
        result.sort((a, b) => b.price - a.price);
        break;
      case 'newest':
      default:
        result.sort((a, b) => b.id - a.id);
        break;
    }

    return result;
  });

  constructor(
    public bookService: BookPublicService,
    public imageService: ImageService,
    private router: Router,
    private authService: AuthService,
    private wishlistService: UserWishListService
  ) {}

  ngOnInit(): void {
    this.bookService.getNewBooks();

    this.authService.isLoggedIn$
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((isLogged) => {
        this.isLoggedIn = isLogged;
      });
  }

  getBookImage(book: BookAllData): string {
    const primary = book.images?.find((i) => i.isPrimary);
    return primary
      ? this.imageService.getUrl(primary.relativePath, primary.storedFileName)
      : 'https://placehold.co/300x400?text=No+Image';
  }

  toggleWishlist(bookId: number, event: MouseEvent) {
    event.stopPropagation(); // prevent opening book page
    this.wishlistService.ToggleWishlist(bookId);
  }

  onSearch(event: any): void {
    this.searchTerm.set(event.target.value);
  }

  onSortChange(event: any): void {
    this.sortOption.set(event.target.value);
  }

  clearFilters(): void {
    this.searchTerm.set('');
    this.sortOption.set('newest');
  }

  goToBookDetails(bookId: number) {
    this.router.navigate(['/books', bookId]);
  }

  changePage(page: number) {
    const pagination = this.bookService.pagination();
    if (pagination && page !== pagination.pageNumber) {
      this.bookService.getNewBooks(page, pagination.pageSize);
    }
  }

  getPageArray(): number[] {
    const pagination = this.bookService.pagination();
    if (!pagination) return [];

    const total = pagination.totalPages;
    const current = pagination.pageNumber;

    const pages: number[] = [];

    for (let i = 1; i <= total; i++) {
      if (i === 1 || i === total || (i >= current - 1 && i <= current + 1)) {
        pages.push(i);
      } else if (i === current - 2 || i === current + 2) {
        pages.push(-1); // use -1 as ellipsis
      }
    }

    return [...new Set(pages)];
  }
}
