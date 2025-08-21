import { Component, viewChild } from '@angular/core';
import { Tag, TagPaginationInfo } from '../../models/tag';
import { ModalComponent } from '../../ui-service/modal/modal.component';
import { TagService } from '../../services/admin/tag.service';

@Component({
  selector: 'app-tag',
  standalone: false,
  templateUrl: './tag.component.html',
  styleUrl: './tag.component.css',
})
export class TagComponent {
  tags: Tag[] = [];
  tag: Tag = {} as Tag;
  deleteId: number = 0;
  pagination: TagPaginationInfo = {
    pageNumber: 1,
    pageSize: 20,
    totalCount: 0,
    totalPages: 1,
  };
  createTagModal = viewChild<ModalComponent>('createTag');
  updateTagModal = viewChild<ModalComponent>('updateTag');
  deleteTagModal = viewChild<ModalComponent>('deleteTag');

  constructor(public tagService: TagService) {
    this.tagService.tags.subscribe((tags) => {
      this.tags = tags;
    });
    this.tagService.pagination.subscribe((pagination) => {
      this.pagination = pagination;
    });
  }

  ngOnInit() {
    // Fetch authors
    this.tagService.getTags(
      this.pagination.pageNumber,
      this.pagination.pageSize
    );
  }

  create() {
    this.createTagModal()!.open();
  }

  update(id: number) {
    this.tagService.getById(id);
    this.updateTagModal()!.open();
  }

  delete(id: number) {
    this.deleteId = id;
    this.deleteTagModal()!.open();
  }

    deleteConfirmed() {
    this.tagService.delete(this.deleteId);
    this.closeDialog('deleteTagModal');
  }

  changePage(page: number) {
    if (page !== this.pagination.pageNumber) {
      this.tagService.getTags(page, this.pagination.pageSize);
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
    (pages);

    return [...new Set(pages)];
  }

  closeDialog(tab: string) {
    switch (tab) {
      case 'createTagModal':
        this.createTagModal()!.close();
        break;
      case 'updateTagModal':
        this.updateTagModal()!.close();
        break;
      case 'deleteTagModal':
        this.deleteTagModal()!.close();
        break;
    }
  }
}
