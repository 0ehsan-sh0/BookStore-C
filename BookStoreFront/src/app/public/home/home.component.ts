import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { BookPublicService } from '../../services/Public/book-public.service';
import { ImageService } from '../../services/image.service';
import { BookAllData } from '../../models/book';
import { Router } from '@angular/router';
import { UserCartService } from '../../services/user/user-cart.service';
import { AlertService } from '../../ui-service/alert.service';

@Component({
  selector: 'app-home',
  standalone: false,
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeComponent implements OnInit {
  newBooks: BookAllData[] = [];
  recommendedBooks: BookAllData[] = [];
  constructor(
    public bookService: BookPublicService,
    public imageService: ImageService,
    private router: Router,
    private cartService: UserCartService,
    private alertService: AlertService
  ) {
    this.bookService.newBooks.subscribe((books) => {
      this.newBooks = books;
    });
    this.bookService.recommendedBooks.subscribe((books) => {
      this.recommendedBooks = books;
    });
  }
  ngOnInit(): void {
    this.bookService.getNewBooks();
    this.bookService.getNewBooks(1, 20, true);
  }

  goToBookDetails(bookId: number) {
    this.router.navigate(['/books', bookId]);
  }

  addToCart(book: BookAllData) {
    this.cartService.addToCart({ ...book, quantity: 1 });
    this.alertService.show('کتاب با موفقیت به سبد خرید اضافه شد');
  }
}
