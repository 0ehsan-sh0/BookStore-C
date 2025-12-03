import { Component } from '@angular/core';
import { BookAllData } from '../../models/book';
import { ActivatedRoute } from '@angular/router';
import { BookPublicService } from '../../services/Public/book-public.service';
import { ImageService } from '../../services/image.service';
import { AlertService } from '../../ui-service/alert.service';
import { UserCartService } from '../../services/user/user-cart.service';

@Component({
  selector: 'app-book-details',
  standalone: false,
  templateUrl: './book-details.component.html',
  styleUrl: './book-details.component.css',
})
export class BookDetailsComponent {
  book: BookAllData | null = null;
  isLoading = true;

  constructor(
    private route: ActivatedRoute,
    private bookService: BookPublicService,
    private imageService: ImageService,
    private alertService: AlertService,
    private cartService: UserCartService
  ) {}

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      const id = Number(idParam);
      this.bookService.getBookById(id);
      this.bookService.book.subscribe((book) => {
        this.book = book;
        this.isLoading = false;
      });
    }
  }

  getPrimaryImage(): string {
    if (!this.book?.images || this.book.images.length === 0) {
      return 'https://placehold.co/400x600?text=No+Image';
    }
    const primary = this.book.images.find((img) => img.isPrimary);
    const img = primary || this.book.images[0];
    return this.imageService.getUrl(img.relativePath, img.storedFileName);
  }

  addToCart(book: BookAllData | null) {
    if (!book) 
      return;
  
    this.cartService.addToCart(book);
    this.alertService.show('کتاب با موفقیت به سبد خرید اضافه شد');
  }
}
