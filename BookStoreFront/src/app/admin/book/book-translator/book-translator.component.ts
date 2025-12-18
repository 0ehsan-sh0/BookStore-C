import { Component, effect } from '@angular/core';
import { Translator } from '../../../models/translator';
import { BookService } from '../../../services/admin/book.service';

@Component({
  selector: 'app-book-translator',
  standalone: false,
  templateUrl: './book-translator.component.html',
  styleUrl: './book-translator.component.css',
})
export class BookTranslatorComponent {
  translators: Translator[] = [];

  constructor(public bookService: BookService) {
    effect(() => {
      this.translators = this.bookService.book().translators ?? [];
    });
  }
}
