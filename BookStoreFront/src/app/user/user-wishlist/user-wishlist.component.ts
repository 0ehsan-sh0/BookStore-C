import { Component, OnInit, inject, DestroyRef } from '@angular/core';
import { BookAllData, BPaginationInfo } from '../../models/book';
import { ImageService } from '../../services/image.service';
import { Router } from '@angular/router';
import { UserWishListService } from '../../services/user/user-wish-list.service';

@Component({
  selector: 'app-user-wishlist',
  standalone: false,
  templateUrl: './user-wishlist.component.html',
  styleUrl: './user-wishlist.component.css',
})
export class UserWishlistComponent implements OnInit {
  private destroyRef = inject(DestroyRef);
  public wishlistService = inject(UserWishListService);
  public imageService = inject(ImageService);
  private router = inject(Router);

  ngOnInit(): void {
    // initial load
    this.wishlistService.getUserWishlist(1, 10);
  }

  toggleWishlist(bookId: number, event: MouseEvent) {
    event.stopPropagation(); // prevent opening book page

    this.wishlistService.ToggleWishlist(bookId);
  }

  // Pagination
  changePage(page: number) {
    const pagination = this.wishlistService.pagination();
    if (page !== pagination.pageNumber) {
      this.wishlistService.getUserWishlist(page, pagination.pageSize);
    }
  }

  getPageArray(): number[] {
    const pagination = this.wishlistService.pagination();
    const total = pagination.totalPages;
    const current = pagination.pageNumber;
    const pages: number[] = [];

    for (let i = 1; i <= total; i++) {
      if (i === 1 || i === total || (i >= current - 1 && i <= current + 1)) {
        pages.push(i);
      } else if (i === current - 2 || i === current + 2) {
        pages.push(-1);
      }
    }

    return [...new Set(pages)];
  }

  // Redirect to details
  goToBookDetails(bookId: number) {
    this.router.navigate(['/books', bookId]);
  }

  // Get book image
  getImage(book: BookAllData): string {
    const img = book.images?.[0];
    if (!img) return 'https://placehold.co/300x400?text=No+Image';
    return this.imageService.getUrl(img.relativePath, img.storedFileName);
  }
}
