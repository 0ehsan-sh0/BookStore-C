import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { BookPublicService } from '../../services/Public/book-public.service';
import { ImageService } from '../../services/image.service';
import { BookAllData } from '../../models/book';

@Component({
  selector: 'app-home',
  standalone: false,
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeComponent implements OnInit {
  newBooks: BookAllData[] = [];
  constructor(
      public bookService: BookPublicService,
      public imageService: ImageService
    ) {
      this.bookService.newBooks.subscribe((books) => {
        this.newBooks = books;
      });
    }
    ngOnInit(): void {
      this.bookService.getNewBooks();
    }
}
