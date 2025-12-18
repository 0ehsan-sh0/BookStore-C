import { Component, effect } from '@angular/core';
import { Tag } from '../../../models/tag';
import { BookService } from '../../../services/admin/book.service';

@Component({
  selector: 'app-book-tag',
  standalone: false,
  templateUrl: './book-tag.component.html',
  styleUrl: './book-tag.component.css',
})
export class BookTagComponent {
  tags: Tag[] = [];

  constructor(public bookService: BookService) {
    effect(() => {
      this.tags = this.bookService.book().tags ?? [];
    });
  }
}
