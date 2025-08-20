import { Component, viewChild } from '@angular/core';
import { Book, BookAllData, BPaginationInfo } from '../../models/book';
import { ModalComponent } from '../../ui-service/modal/modal.component';
import { CategoryService } from '../../services/admin/category.service';
import { BookService } from '../../services/admin/book.service';
import { ImageService } from '../../services/image.service';
import { Category } from '../../models/category';

@Component({
  selector: 'app-book',
  standalone: false,
  templateUrl: './book.component.html',
  styleUrl: './book.component.css',
})
export class BookComponent {
  books: BookAllData[] = [];
  book: BookAllData = {} as BookAllData;
  deleteId: number = 0;
  pagination: BPaginationInfo = {
    pageNumber: 1,
    pageSize: 20,
    totalCount: 0,
    totalPages: 1,
  };
  createBookModal = viewChild<ModalComponent>('createBook');
  updateBookModal = viewChild<ModalComponent>('updateBook');
  deleteBookModal = viewChild<ModalComponent>('deleteBook');
  bookCategories = viewChild<ModalComponent>('bookCategories');
  bookTranslators = viewChild<ModalComponent>('bookTranslators');

  constructor(
    public bookService: BookService,
    public imageService: ImageService
  ) {
    this.bookService.books.subscribe((books) => {
      this.books = books;
    });
    this.bookService.pagination.subscribe((pagination) => {
      this.pagination = pagination;
    });
  }

  ngOnInit() {
    // Fetch books
    this.bookService.getBooks(
      this.pagination.pageNumber,
      this.pagination.pageSize
    );
  }

  create() {
    this.createBookModal()!.open();
  }

  update(id: number) {
    this.bookService.getById(id);
    this.updateBookModal()!.open();
  }

  delete(id: number) {
    this.deleteId = id;
    this.deleteBookModal()!.open();
  }

  deleteConfirmed() {
    this.bookService.delete(this.deleteId);
    this.closeDialog('deleteBookModal');
  }

  showCategories(id : number) {
    this.bookService.getById(id);
    this.bookCategories()!.open();
  }

  showTranslators(id : number) {
    this.bookService.getById(id);
    this.bookTranslators()!.open();
  }

  changePage(page: number) {
    if (page !== this.pagination.pageNumber) {
      this.bookService.getBooks(page, this.pagination.pageSize);
    }
  }

  getPageArray(): number[] {
    const total = this.pagination.totalPages;
    const current = this.pagination.pageNumber;

    const pages: number[] = [];

    for (let i = 1; i <= total; i++) {
      if (i === 1 || i === total || (i >= current - 1 && i <= current + 1)) {
        pages.push(i);
      } else if (i === current - 2 || i === current + 2) {
        pages.push(-1); // use -1 as ellipsis
      }
    }
    console.log(pages);

    return [...new Set(pages)];
  }

  closeDialog(tab: string) {
    switch (tab) {
      case 'createBookModal':
        this.createBookModal()!.close();
        break;
      case 'updateBookModal':
        this.updateBookModal()!.close();
        break;
      case 'deleteBookModal':
        this.deleteBookModal()!.close();
        break;
    }
  }
}
