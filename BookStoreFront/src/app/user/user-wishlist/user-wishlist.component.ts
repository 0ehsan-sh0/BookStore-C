import { Component, OnInit } from '@angular/core';
import { BookAllData, BPaginationInfo } from '../../models/book';
import { AuthService } from '../../services/auth.service';
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
  wishlist: BookAllData[] = [];

  pagination: BPaginationInfo = {
    pageNumber: 1,
    pageSize: 10,
    totalCount: 0,
    totalPages: 1,
  };

  constructor(
    private wishlistService: UserWishListService,
    private imageService: ImageService,
    private router: Router
  ) {}

  ngOnInit(): void {
    // initial load
    this.wishlistService.getUserWishlist(
      this.pagination.pageNumber,
      this.pagination.pageSize
    );

    // subscribe to data
    this.wishlistService.wishlist.subscribe((books) => {
      this.wishlist = books ?? [];
    });

    // subscribe to pagination
    this.wishlistService.pagination.subscribe((p) => {
      this.pagination = p ?? this.pagination;
    });
  }

  toggleWishlist(bookId: number, event: MouseEvent) {
    event.stopPropagation(); // prevent opening book page

    this.wishlistService.ToggleWishlist(bookId);
  }

  // Pagination
  changePage(page: number) {
    if (page !== this.pagination.pageNumber) {
      this.wishlistService.getUserWishlist(page, this.pagination.pageSize);
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
