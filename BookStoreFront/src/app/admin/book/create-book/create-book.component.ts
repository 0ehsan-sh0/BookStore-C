import { Component, effect, output, viewChild } from '@angular/core';
import { Category } from '../../../models/category';
import { Tag } from '../../../models/tag';
import { Translator } from '../../../models/translator';
import { Author } from '../../../models/author';
import { NgForm } from '@angular/forms';
import { BookService } from '../../../services/admin/book.service';
import { AuthorService } from '../../../services/admin/author.service';
import { TranslatorService } from '../../../services/admin/translator.service';
import { CategoryService } from '../../../services/admin/category.service';
import { TagService } from '../../../services/admin/tag.service';
import { CreateBookRequest } from '../../../models/book';

@Component({
  selector: 'app-create-book',
  standalone: false,
  templateUrl: './create-book.component.html',
  styleUrl: './create-book.component.css',
})
export class CreateBookComponent {
  created = output();
  categories: Category[] = [];
  tags: Tag[] = [];
  translators: Translator[] = [];
  authors: Author[] = [];
  errors: string[] = [];
  form = viewChild<NgForm>('form');

  searchAuthorText = '';
  searchTranslatorText = '';
  searchCategoryText = '';
  searchTagText = '';

  selectedAuthor: number | null = null;
  selectedTranslators: Translator[] = [];
  selectedCategories: Category[] = [];
  selectedTags: Tag[] = [];
  selectedFiles: File[] = [];

  constructor(
    public bookService: BookService,
    public authorService: AuthorService,
    public translatorService: TranslatorService,
    public categoryService: CategoryService,
    public tagService: TagService
  ) {
    effect(() => {
      this.errors = this.bookService.createErrors();
    });

    // reactively track creation
    effect(() => {
      const isCreated = this.bookService.created();

      if (isCreated) {
        this.created.emit();
        this.form()?.reset();
        this.bookService.created.set(false); // reset the flag so effect won't fire again
      }
    });

    this.authorService.authors.subscribe((authors) => {
      this.authors = authors;
    });
    this.translatorService.translators.subscribe((translators) => {
      this.translators = translators;
    });
    this.categoryService.categories.subscribe((categories) => {
      this.categories = categories;
    });
    this.tagService.tags.subscribe((tags) => {
      this.tags = tags;
    });
    this.authorService.getAuthors();
    this.translatorService.getTranslators();
    this.categoryService.getCategories();
    this.tagService.getTags();
  }

  onSubmit(form: NgForm) {
    const formData = new FormData();

    // Simple fields
    formData.append('name', form.value.name);
    formData.append('englishName', form.value.englishName || '');
    formData.append('description', form.value.description || '');
    formData.append('price', form.value.price);
    formData.append('printSeries', form.value.printSeries);
    formData.append('isbn', form.value.isbn);
    formData.append('coverType', form.value.coverType);
    formData.append('format', form.value.format);
    formData.append('pages', form.value.pages);
    formData.append('publishYear', form.value.publishYear);
    formData.append('publisher', form.value.publisher);
    formData.append('authorId', this.selectedAuthor!.toString());

    // Files
    this.selectedFiles.forEach((file, index) => {
      formData.append('images', file); // use same key as backend expects
    });

    // Arrays
    this.selectedTranslators.forEach((t) =>
      formData.append('translators', t.id.toString())
    );
    this.selectedCategories.forEach((c) =>
      formData.append('categories', c.id.toString())
    );
    this.selectedTags.forEach((t) => formData.append('tags', t.id.toString()));

    // Send
    this.bookService.create(formData);
  }

  onFilesSelected(event: any) {
    this.selectedFiles = Array.from(event.target.files);
  }

  onAuthorSearch() {
    this.authorService.getAuthors(1, 20, this.searchAuthorText);
  }

  // -------- tag
  onTagSearch() {
    this.tagService.getTags(1, 20, this.searchTagText);
  }

  isTagSelected(tag: Tag): boolean {
    return this.selectedTags.some((t) => t.id === tag.id);
  }

  toggleTagSelection(tag: Tag) {
    const index = this.selectedTags.findIndex((t) => t.id === tag.id);
    if (index > -1) {
      // Already selected → remove
      this.selectedTags.splice(index, 1);
    } else {
      // Not selected → add
      this.selectedTags.push(tag);
    }
  }
  // -------- end tag

  // -------- category
  onCategorySearch() {
    this.categoryService.getCategories(1, 20, this.searchCategoryText);
  }

  isCategorySelected(category: Category): boolean {
    return this.selectedCategories.some((c) => c.id === category.id);
  }

  toggleCategorySelection(category: Category) {
    const index = this.selectedCategories.findIndex(
      (c) => c.id === category.id
    );
    if (index > -1) {
      // Already selected → remove
      this.selectedCategories.splice(index, 1);
    } else {
      // Not selected → add
      this.selectedCategories.push(category);
    }
  }
  // -------- end category

  // -------- translator
  onTranslatorSearch() {
    this.translatorService.getTranslators(1, 20, this.searchTranslatorText);
  }

  isTranslatorSelected(translator: Translator): boolean {
    return this.selectedTranslators.some((t) => t.id === translator.id);
  }

  toggleTranslatorSelection(translator: Translator) {
    const index = this.selectedTranslators.findIndex(
      (t) => t.id === translator.id
    );
    if (index > -1) {
      // Already selected → remove
      this.selectedTranslators.splice(index, 1);
    } else {
      // Not selected → add
      this.selectedTranslators.push(translator);
    }
  }

  // -------- end translator
}
