import {
  Component,
  ElementRef,
  OnInit,
  ViewChild,
  inject,
  DestroyRef,
} from '@angular/core';
import { BookPublicService } from '../../services/Public/book-public.service';
import { ImageService } from '../../services/image.service';
import { BookAllData } from '../../models/book';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { UserWishListService } from '../../services/user/user-wish-list.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-home',
  standalone: false,
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeComponent implements OnInit {
  private destroyRef = inject(DestroyRef);
  isLoggedIn = false;

  constructor(
    public bookService: BookPublicService,
    public imageService: ImageService,
    private router: Router,
    private authService: AuthService,
    private wishlistService: UserWishListService
  ) {}

  ngOnInit(): void {
    this.bookService.getNewBooks(1, 12);
    this.bookService.getNewBooks(1, 10, true);

    this.authService.isLoggedIn$
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((isLogged) => {
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
