import { Component, effect, viewChild } from '@angular/core';
import { AuthorService } from '../../services/admin/author.service';
import { APaginationInfo, Author } from '../../models/author';
import { ModalComponent } from '../../ui-service/modal/modal.component';

@Component({
  selector: 'app-admin-author',
  standalone: false,
  templateUrl: './author.component.html',
  styleUrl: './author.component.css',
})
export class AuthorComponent {
  authors: Author[] = [];
  author: Author = {} as Author;
  deleteId: number = 0;
  pagination: APaginationInfo = {
    pageNumber: 1,
    pageSize: 20,
    totalCount: 0,
    totalPages: 1,
  };
  createAuthorModal = viewChild<ModalComponent>('createAuthor');
  updateAuthorModal = viewChild<ModalComponent>('updateAuthor');
  deleteAuthorModal = viewChild<ModalComponent>('deleteAuthor');

  constructor(public authorService: AuthorService) {
    this.authorService.authors.subscribe((authors) => {
      this.authors = authors;
    });
    this.authorService.pagination.subscribe((pagination) => {
      this.pagination = pagination;
    });
  }

  ngOnInit() {
    // Fetch authors
    this.authorService.getAuthors(
      this.pagination.pageNumber,
      this.pagination.pageSize
    );
  }

  create() {
    this.createAuthorModal()!.open();
  }

  update(id: number) {
    this.authorService.getById(id);
    this.updateAuthorModal()!.open();
  }

  delete(id: number) {
    this.deleteId = id;
    this.deleteAuthorModal()!.open();
  }

  deleteConfirmed() {
    this.authorService.delete(this.deleteId);
    this.closeDialog('deleteAuthorModal');
  }

  changePage(page: number) {
    if (page !== this.pagination.pageNumber) {
      this.authorService.getAuthors(page, this.pagination.pageSize);
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
      case 'createAuthorModal':
        this.createAuthorModal()!.close();
        break;
      case 'updateAuthorModal':
        this.updateAuthorModal()!.close();
        break;
      case 'deleteAuthorModal':
        this.deleteAuthorModal()!.close();
        break;
    }
  }
}
