import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { BookPublicService } from '../../services/Public/book-public.service';
import { ImageService } from '../../services/image.service';
import { BookAllData } from '../../models/book';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { UserWishListService } from '../../services/user/user-wish-list.service';

@Component({
  selector: 'app-home',
  standalone: false,
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeComponent implements OnInit {
  newBooks: BookAllData[] = [];
  recommendedBooks: BookAllData[] = [];
  isLoggedIn = false;

  constructor(
    public bookService: BookPublicService,
    public imageService: ImageService,
    private router: Router,
    private authService: AuthService,
    private wishlistService: UserWishListService
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
    this.authService.isLoggedIn$.subscribe((isLogged) => {
      this.isLoggedIn = isLogged;
    });
  }

  goToBookDetails(bookId: number) {
    this.router.navigate(['/books', bookId]);
  }

  toggleWishlist(bookId: number, event: MouseEvent) {
    event.stopPropagation(); // prevent opening book page

    this.wishlistService.ToggleWishlist(bookId);
  }
}
