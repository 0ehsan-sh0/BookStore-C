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
import { Book, UpdateBookRequest } from '../../../models/book';

@Component({
  selector: 'app-update-book',
  standalone: false,
  templateUrl: './update-book.component.html',
  styleUrl: './update-book.component.css',
})
export class UpdateBookComponent {
  updated = output();
  book: Book = {} as Book;
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

  constructor(
    public bookService: BookService,
    public authorService: AuthorService,
    public translatorService: TranslatorService,
    public categoryService: CategoryService,
    public tagService: TagService
  ) {
    effect(() => {
      this.errors = this.bookService.updateErrors();
    });

    // reactively track creation
    effect(() => {
      const isUdated = this.bookService.updated();

      if (isUdated) {
        this.updated.emit();
        this.form()?.reset();
        this.bookService.updated.set(false); // reset the flag so effect won't fire again
      }
    });

    this.bookService.book.subscribe((book) => {
      this.book = book;
      this.selectedAuthor = book.author ? book.author.id : null;
      this.selectedTranslators = book.translators ?? [];
      this.selectedCategories = book.categories ?? [];
      this.selectedTags = book.tags ?? [];
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
    const book: UpdateBookRequest = {
      name: form.value.name,
      englishName: form.value.englishName || '',
      description: form.value.description || '',
      price: form.value.price,
      printSeries: form.value.printSeries,
      isbn: form.value.isbn,
      coverType: form.value.coverType,
      format: form.value.format,
      pages: form.value.pages,
      publishYear: form.value.publishYear,
      publisher: form.value.publisher,
      authorId: this.selectedAuthor!,
      translators: this.selectedTranslators.map((t) => t.id),
      categories: this.selectedCategories.map((c) => c.id),
      tags: this.selectedTags.map((t) => t.id),
    };

    // Send
    this.bookService.update(book, this.book.id);
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
