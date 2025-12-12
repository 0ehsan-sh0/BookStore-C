import { Component } from '@angular/core';
import { AuthorDetails } from '../../models/author';
import { BookAllData, BPaginationInfo } from '../../models/book';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthorPublicService } from '../../services/Public/author-public.service';
import { ImageService } from '../../services/image.service';
import { AuthService } from '../../services/auth.service';
import { UserWishListService } from '../../services/user/user-wish-list.service';

@Component({
  selector: 'app-author-details',
  standalone: false,
  templateUrl: './author-details.component.html',
  styleUrl: './author-details.component.css',
})
export class AuthorDetailsComponent {
  authorId!: number;

  author: AuthorDetails = {} as AuthorDetails;
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
    private authorService: AuthorPublicService,
    public imageService: ImageService,
    private authService: AuthService,
    private wishlistService: UserWishListService
  ) {}

  ngOnInit(): void {
    this.authorId = Number(this.route.snapshot.paramMap.get('id'));

    this.loadAuthor();

    this.authService.isLoggedIn$.subscribe((isLogged) => {
      this.isLoggedIn = isLogged;
    });
  }

  loadAuthor(): void {
    this.authorService.getAuthorDetails(
      this.authorId,
      this.pagination.pageNumber,
      this.pagination.pageSize
    );

    this.authorService.authorDetails.subscribe((details) => {
      if (!details?.author) return;

      this.author = details;
      this.books = details.books.books ?? [];
      this.pagination = details.books.pagination ?? this.pagination;

      this.filteredBooks = [...this.books];
    });
  }

  // -----------------------------
  // UI Actions
  // -----------------------------

  toggleWishlist(bookId: number, event: MouseEvent) {
    event.stopPropagation();
    this.wishlistService.ToggleWishlist(bookId);
  }

  goToBookDetails(id: number) {
    this.router.navigate(['/books', id]);
  }

  changePage(page: number) {
    if (page === this.pagination.pageNumber) return;
    this.authorService.getAuthorDetails(
      this.authorId,
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
