import { Component, OnInit } from '@angular/core';
import { BookAllData } from '../../models/book';
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
    this.bookService.getNewBooks();
  }

  loadBooks(): void {
    this.bookService.newBooks.subscribe((data) => {
      this.books = data;
      this.filteredBooks = [...this.books];
      this.applySort();
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
}
