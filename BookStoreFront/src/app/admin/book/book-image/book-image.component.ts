import { Component, ViewChild } from '@angular/core';
import { Image } from '../../../models/image';
import { BookService } from '../../../services/admin/book.service';
import { ImageService } from '../../../services/image.service';
import { Book } from '../../../models/book';

@Component({
  selector: 'app-book-image',
  standalone: false,
  templateUrl: './book-image.component.html',
  styleUrl: './book-image.component.css',
})
export class BookImageComponent {
  images: Image[] = [];
  book: Book = {} as Book;

  constructor(
    public bookService: BookService,
    public imageService: ImageService
  ) {
    this.bookService.book.subscribe((book) => {
      this.images = book.images ?? [];
      this.book = book;
      (book.images);
    });
  }
}
