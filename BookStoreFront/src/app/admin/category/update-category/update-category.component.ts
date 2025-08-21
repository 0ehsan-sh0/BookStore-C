import { Component, effect, output, viewChild } from '@angular/core';
import { Category, UpdateCategoryRequest } from '../../../models/category';
import { NgForm } from '@angular/forms';
import { CategoryService } from '../../../services/admin/category.service';

@Component({
  selector: 'app-update-category',
  standalone: false,
  templateUrl: './update-category.component.html',
  styleUrl: './update-category.component.css',
})
export class UpdateCategoryComponent {
  updated = output();
  categories: Category[] = [];
  errors: string[] = [];
  form = viewChild<NgForm>('form');
  category: Category = {} as Category;

  constructor(private categoryService: CategoryService) {
    this.categoryService.categories.subscribe((categories) => {
      this.categories = categories;
    });
    this.categoryService.category.subscribe((category) => {
      this.category = category;
    });
    // reactively track errors
    effect(() => {
      this.errors = this.categoryService.updateErrors();
    });

    // reactively track creation
    effect(() => {
      const isUpdated = this.categoryService.updated();

      if (isUpdated) {
        this.updated.emit();
        this.form()?.reset();
        this.categoryService.updated.set(false); // reset the flag so effect won't fire again
      }
    });
  }
  onSubmit(form: NgForm) {
    let category: UpdateCategoryRequest = {
      name: form.value.name,
      url: form.value.url,
      mainCategoryId: form.value.mainCategoryId
        ? form.value.mainCategoryId
        : null,
    };
    this.categoryService.update(category, this.category.id);
  }
}
