import { Component, viewChild } from '@angular/core';
import { TPaginationInfo, Translator } from '../../models/translator';
import { ModalComponent } from '../../ui-service/modal/modal.component';
import { TranslatorService } from '../../services/admin/translator.service';

@Component({
  selector: 'app-admin-translator',
  standalone: false,
  templateUrl: './translator.component.html',
  styleUrl: './translator.component.css',
})
export class TranslatorComponent {
  translators: Translator[] = [];
  translator: Translator = {} as Translator;
  deleteId: number = 0;
  pagination: TPaginationInfo = {
    pageNumber: 1,
    pageSize: 20,
    totalCount: 0,
    totalPages: 1,
  };
  createTranslatorModal = viewChild<ModalComponent>('createTranslator');
  updateTranslatorModal = viewChild<ModalComponent>('updateTranslator');
  deleteTranslatorModal = viewChild<ModalComponent>('deleteTranslator');

  constructor(public translatorService: TranslatorService) {
    this.translatorService.translators.subscribe((translators) => {
      this.translators = translators;
    });
    this.translatorService.pagination.subscribe((pagination) => {
      this.pagination = pagination;
    });
  }

  ngOnInit() {
    // Fetch authors
    this.translatorService.getTranslators(
      this.pagination.pageNumber,
      this.pagination.pageSize
    );
  }

  create() {
    this.createTranslatorModal()!.open();
  }

  update(id: number) {
    this.translatorService.getById(id);
    this.updateTranslatorModal()!.open();
  }

  delete(id: number) {
    this.deleteId = id;
    this.deleteTranslatorModal()!.open();
  }
  
  deleteConfirmed() {
    this.translatorService.delete(this.deleteId);
    this.closeDialog('deleteTranslatorModal');
  }

  changePage(page: number) {
    if (page !== this.pagination.pageNumber) {
      this.translatorService.getTranslators(page, this.pagination.pageSize);
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
      case 'createTranslatorModal':
        this.createTranslatorModal()!.close();
        break;
      case 'updateTranslatorModal':
        this.updateTranslatorModal()!.close();
        break;
      case 'deleteTranslatorModal':
        this.deleteTranslatorModal()!.close();
        break;
    }
  }
}
