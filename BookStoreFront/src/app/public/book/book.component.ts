import { Component, OnInit } from '@angular/core';
import { BookAllData, BPaginationInfo } from '../../models/book';
import { BookPublicService } from '../../services/Public/book-public.service';
import { ImageService } from '../../services/image.service';
import { Router } from '@angular/router';
import { AlertService } from '../../ui-service/alert.service';
import { UserCartService } from '../../services/user/user-cart.service';

@Component({
  selector: 'app-book',
  standalone: false,
  templateUrl: './book.component.html',
  styleUrl: './book.component.css',
})
export class BookComponent implements OnInit {
  books: BookAllData[] = [];
  filteredBooks: BookAllData[] = [];
  pagination: BPaginationInfo = {
    pageNumber: 1,
    pageSize: 20,
    totalCount: 0,
    totalPages: 1,
  };

  searchTerm: string = '';
  sortOption: string = 'newest';

  constructor(
    private bookService: BookPublicService,
    public imageService: ImageService,
    private router: Router,
    private cartService: UserCartService,
    private alertService: AlertService
  ) {}

  ngOnInit(): void {
    this.loadBooks();
    this.bookService.getNewBooks(
      this.pagination.pageNumber,
      this.pagination.pageSize
    );
  }

  loadBooks(): void {
    this.bookService.newBooks.subscribe((data) => {
      this.books = data;
      this.filteredBooks = [...this.books];
      this.applySort();
    });

    this.bookService.newBooksPagination.subscribe((pagination) => {
      if (pagination) {
        this.pagination = pagination;
      }
    });
  }

  getBookImage(book: BookAllData): string {
    const primary = book.images?.find((i) => i.isPrimary);
    return primary
      ? this.imageService.getUrl(primary.relativePath, primary.storedFileName)
      : 'https://placehold.co/300x400?text=No+Image';
  }

  onSearch(): void {
    const term = this.searchTerm.toLowerCase();
    this.filteredBooks = this.books.filter(
      (b) =>
        b.name.toLowerCase().includes(term) ||
        b.author?.name?.toLowerCase().includes(term)
    );
    this.applySort();
  }

  onSortChange(): void {
    this.applySort();
  }

  applySort(): void {
    switch (this.sortOption) {
      case 'priceLow':
        this.filteredBooks.sort((a, b) => a.price - b.price);
        break;
      case 'priceHigh':
        this.filteredBooks.sort((a, b) => b.price - a.price);
        break;
      case 'newest':
      default:
        this.filteredBooks.sort((a, b) => b.id - a.id);
        break;
    }
  }

  clearFilters(): void {
    this.searchTerm = '';
    this.sortOption = 'newest';
    this.filteredBooks = [...this.books];
  }

  goToBookDetails(bookId: number) {
    this.router.navigate(['/books', bookId]);
  }

  addToCart(book: BookAllData) {
    this.cartService.addToCart({ ...book, quantity: 1 });
    this.alertService.show('کتاب با موفقیت به سبد خرید اضافه شد');
  }

  changePage(page: number) {
    if (page !== this.pagination.pageNumber) {
      this.bookService.getNewBooks(page, this.pagination.pageSize);
    }
  }

  getPageArray(): number[] {
    const total = this.pagination.totalPages;
    const current = this.pagination.pageNumber;

    const pages: number[] = [];

    for (let i = 1; i <= total; i++) {
      if (i === 1 || i === total || (i >= current - 1 && i <= current + 1)) {
        pages.push(i);
      } else if (i === current - 2 || i === current + 2) {
        pages.push(-1); // use -1 as ellipsis
      }
    }
    console.log(pages);

    return [...new Set(pages)];
  }
}
