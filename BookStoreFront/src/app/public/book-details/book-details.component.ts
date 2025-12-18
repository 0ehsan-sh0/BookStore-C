import { Component, OnInit, inject, DestroyRef } from '@angular/core';
import { BookAllData } from '../../models/book';
import { ActivatedRoute } from '@angular/router';
import { BookPublicService } from '../../services/Public/book-public.service';
import { ImageService } from '../../services/image.service';
import { AlertService } from '../../ui-service/alert.service';
import { UserCartService } from '../../services/user/user-cart.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-book-details',
  standalone: false,
  templateUrl: './book-details.component.html',
  styleUrl: './book-details.component.css',
})
export class BookDetailsComponent implements OnInit {
  private destroyRef = inject(DestroyRef);
  isLoading = true;

  constructor(
    private route: ActivatedRoute,
    public bookService: BookPublicService,
    private imageService: ImageService,
    private alertService: AlertService,
    private cartService: UserCartService
  ) {}

  ngOnInit(): void {
    this.route.paramMap
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((params) => {
        const id = Number(params.get('id'));
        if (id) {
          this.bookService.getBookById(id);
          this.isLoading = false;
        }
      });
  }

  getPrimaryImage(): string {
    const book = this.bookService.details();
    if (!book?.images || book.images.length === 0) {
      return 'https://placehold.co/400x600?text=No+Image';
    }
    const primary = book.images.find((img) => img.isPrimary);
    const img = primary || book.images[0];
    return this.imageService.getUrl(img.relativePath, img.storedFileName);
  }

  addToCart(book: BookAllData | null) {
    if (!book) return;
    this.cartService.addToCart(book);
    this.alertService.show('کتاب با موفقیت به سبد خرید اضافه شد');
  }
}
