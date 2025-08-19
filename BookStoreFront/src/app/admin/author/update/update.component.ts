import { Component, effect, input, output, viewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { AuthorService } from '../../../services/admin/author.service';
import { Author, UpdateAuthorRequest } from '../../../models/author';

@Component({
  selector: 'app-update-author',
  standalone: false,
  templateUrl: './update.component.html',
  styleUrl: './update.component.css',
})
export class UpdateComponent {
  updated = output();
  errors: string[] = [];
  form = viewChild<NgForm>('form');
  author: Author = {} as Author;

  constructor(private authorService: AuthorService) {

    this.authorService.author.subscribe((author) => {
      this.author = author;
    });
    // reactively track errors
    effect(() => {
      this.errors = this.authorService.updateErrors();
    });

    // reactively track creation
    effect(() => {
      const isUpdated = this.authorService.updated();

      if (isUpdated) {
        this.updated.emit();
        this.form()?.reset();
        this.authorService.updated.set(false); // reset the flag so effect won't fire again
      }
    });
  }

  onSubmit(form: NgForm) {
    let author: UpdateAuthorRequest = {
      name: form.value.name,
      description: form.value.description,
    };
    this.authorService.update(author, this.author.id);
  }
}
