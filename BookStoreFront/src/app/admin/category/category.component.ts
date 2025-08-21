import { Component, viewChild } from '@angular/core';
import { Category, CPaginationInfo } from '../../models/category';
import { ModalComponent } from '../../ui-service/modal/modal.component';
import { CategoryService } from '../../services/admin/category.service';

@Component({
  selector: 'app-category',
  standalone: false,
  templateUrl: './category.component.html',
  styleUrl: './category.component.css',
})
export class CategoryComponent {
  categories: Category[] = [];
  category: Category = {} as Category;
  mainCategoryMap: Record<number, string> = {};
  deleteId: number = 0;
  pagination: CPaginationInfo = {
    pageNumber: 1,
    pageSize: 20,
    totalCount: 0,
    totalPages: 1,
  };
  createCategoryModal = viewChild<ModalComponent>('createCategory');
  updateCategoryModal = viewChild<ModalComponent>('updateCategory');
  deleteCategoryModal = viewChild<ModalComponent>('deleteCategory');

  constructor(public categoryService: CategoryService) {
    this.categoryService.categories.subscribe((categories) => {
      this.categories = categories;
      this.mainCategoryMap = {};
      this.categories.forEach((c) => {
        this.mainCategoryMap[c.id] = c.url;
      });
    });
    this.categoryService.pagination.subscribe((pagination) => {
      this.pagination = pagination;
    });
  }

  ngOnInit() {
    // Fetch authors
    this.categoryService.getCategories(
      this.pagination.pageNumber,
      this.pagination.pageSize
    );
  }

  create() {
    this.createCategoryModal()!.open();
  }

  update(id: number) {
    this.categoryService.getById(id);
    this.updateCategoryModal()!.open();
  }

  delete(id: number) {
    this.deleteId = id;
    this.deleteCategoryModal()!.open();
  }

  deleteConfirmed() {
    this.categoryService.delete(this.deleteId);
    this.closeDialog('deleteCategoryModal');
  }

  changePage(page: number) {
    if (page !== this.pagination.pageNumber) {
      this.categoryService.getCategories(page, this.pagination.pageSize);
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
      case 'createCategoryModal':
        this.createCategoryModal()!.close();
        break;
      case 'updateCategoryModal':
        this.updateCategoryModal()!.close();
        break;
      case 'deleteCategoryModal':
        this.deleteCategoryModal()!.close();
        break;
    }
  }
}
