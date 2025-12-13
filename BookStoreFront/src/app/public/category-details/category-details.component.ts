import { Component, OnInit } from '@angular/core';
import { CategoryDetails } from '../../models/category';
import { BookAllData, BPaginationInfo } from '../../models/book';
import { ActivatedRoute, Router } from '@angular/router';
import { CategoryPublicService } from '../../services/Public/category-public.service';
import { ImageService } from '../../services/image.service';
import { AuthService } from '../../services/auth.service';
import { UserWishListService } from '../../services/user/user-wish-list.service';

@Component({
  selector: 'app-category-details',
  standalone: false,
  templateUrl: './category-details.component.html',
  styleUrl: './category-details.component.css',
})
export class CategoryDetailsComponent implements OnInit {
  categoryUrl!: string;

  category: CategoryDetails = {} as CategoryDetails;
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
    private categoryService: CategoryPublicService,
    public imageService: ImageService,
    private authService: AuthService,
    private wishlistService: UserWishListService
  ) {}

  ngOnInit(): void {
    // Category uses URL slug as ID
    this.route.paramMap.subscribe((params) => {
      this.categoryUrl = params.get('id') || '';
      this.loadCategory();
    });

    this.authService.isLoggedIn$.subscribe((isLogged) => {
      this.isLoggedIn = isLogged;
    });
  }

  loadCategory(): void {
    this.categoryService.getCategoryDetails(
      this.categoryUrl,
      this.pagination.pageNumber,
      this.pagination.pageSize
    );

    this.categoryService.categoryDetails.subscribe((details) => {
      if (!details?.category) return;

      this.category = details;
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
    this.categoryService.getCategoryDetails(
      this.categoryUrl,
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
