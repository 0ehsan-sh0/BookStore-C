import { Component, effect, output, viewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { CategoryService } from '../../../services/admin/category.service';
import { CreateAuthorRequest } from '../../../models/author';
import { Category, CreateCategoryRequest } from '../../../models/category';

@Component({
  selector: 'app-create-category',
  standalone: false,
  templateUrl: './create-category.component.html',
  styleUrl: './create-category.component.css',
})
export class CreateCategoryComponent {
  created = output();
  categories: Category[] = [];
  errors: string[] = [];
  form = viewChild<NgForm>('form');

  constructor(private categoryService: CategoryService) {
    this.categoryService.categories.subscribe((categories) => {
      this.categories = categories;
    });
    // reactively track errors
    effect(() => {
      this.errors = this.categoryService.createErrors();
    });

    // reactively track creation
    effect(() => {
      const isCreated = this.categoryService.created();

      if (isCreated) {
        this.created.emit();
        this.form()?.reset();
        this.categoryService.created.set(false); // reset the flag so effect won't fire again
      }
    });
  }
  onSubmit(form: NgForm) {
    let category: CreateCategoryRequest = {
      name: form.value.name,
      url: form.value.url,
      mainCategoryId: form.value.mainCategoryId ? form.value.mainCategoryId : null,
    };
    this.categoryService.create(category);
  }
}
