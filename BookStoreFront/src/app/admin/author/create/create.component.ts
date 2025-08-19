import { Component, effect, input, output, viewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { CreateAuthorRequest } from '../../../models/author';
import { AuthorService } from '../../../services/admin/author.service';
import { FlaskRound } from 'lucide-angular';

@Component({
  selector: 'app-create-author',
  standalone: false,
  templateUrl: './create.component.html',
  styleUrl: './create.component.css',
})
export class CreateComponent {
  created = output();
  errors: string[] = [];
  form = viewChild<NgForm>('form');

  constructor(private authorService: AuthorService) {
    // reactively track errors
    effect(() => {
      this.errors = this.authorService.createErrors();
    });

    // reactively track creation
    effect(() => {
      const isCreated = this.authorService.created();
      
      if (isCreated) {
        this.created.emit();
        this.form()?.reset();
        this.authorService.created.set(false); // reset the flag so effect won't fire again
      }
    });
  }
  onSubmit(form: NgForm) {
    let author: CreateAuthorRequest = {
      name: form.value.name,
      description: form.value.description,
    };
    this.authorService.create(author);
  }
}
