import { Component, OnInit } from '@angular/core';
import { TagDetails } from '../../models/tag';
import { BookAllData, BPaginationInfo } from '../../models/book';
import { ActivatedRoute, Router } from '@angular/router';
import { TagPublicService } from '../../services/Public/tag-public.service';
import { ImageService } from '../../services/image.service';
import { AuthService } from '../../services/auth.service';
import { UserWishListService } from '../../services/user/user-wish-list.service';

@Component({
  selector: 'app-tag-details',
  standalone: false,
  templateUrl: './tag-details.component.html',
  styleUrl: './tag-details.component.css',
})
export class TagDetailsComponent implements OnInit {
  tagUrl!: string;

  tag: TagDetails = {} as TagDetails;
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
    private tagService: TagPublicService,
    public imageService: ImageService,
    private authService: AuthService,
    private wishlistService: UserWishListService
  ) {}

  ngOnInit(): void {
    // Tag uses URL slug as ID
    this.route.paramMap.subscribe((params) => {
      this.tagUrl = params.get('id') || '';
      this.loadTag();
    });

    this.authService.isLoggedIn$.subscribe((isLogged) => {
      this.isLoggedIn = isLogged;
    });
  }

  loadTag(): void {
    this.tagService.getTagDetails(
      this.tagUrl,
      this.pagination.pageNumber,
      this.pagination.pageSize
    );

    this.tagService.tagDetails.subscribe((details) => {
      if (!details?.tag) return;

      this.tag = details;
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
    this.tagService.getTagDetails(this.tagUrl, page, this.pagination.pageSize);
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
