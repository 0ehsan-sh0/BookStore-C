import { Component } from '@angular/core';
import { Category } from '../../../models/category';
import { CategoryService } from '../../../services/admin/category.service';
import { BookService } from '../../../services/admin/book.service';

@Component({
  selector: 'app-book-category',
  standalone: false,
  templateUrl: './book-category.component.html',
  styleUrl: './book-category.component.css',
})
export class BookCategoryComponent {
  categories: Category[] = [];
  mainCategoryMap: Record<number, string> = {};

  constructor(public bookService: BookService) {
    this.bookService.book.subscribe((book) => {
      this.categories = book.categories ?? [];
      this.mainCategoryMap = {};
      this.categories.forEach((c) => {
        this.mainCategoryMap[c.id] = c.url;
      });
    });
  }
}
