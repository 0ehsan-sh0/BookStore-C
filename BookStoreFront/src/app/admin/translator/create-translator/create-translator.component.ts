import { Component, effect, output, viewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { TranslatorService } from '../../../services/admin/translator.service';
import { CreateTranslatorRequest } from '../../../models/translator';

@Component({
  selector: 'app-create-translator',
  standalone: false,
  templateUrl: './create-translator.component.html',
  styleUrl: './create-translator.component.css',
})
export class CreateTranslatorComponent {
  created = output();
  errors: string[] = [];
  form = viewChild<NgForm>('form');

  constructor(private translatorService: TranslatorService) {
    // reactively track errors
    effect(() => {
      this.errors = this.translatorService.createErrors();
    });

    // reactively track creation
    effect(() => {
      const isCreated = this.translatorService.created();

      if (isCreated) {
        this.created.emit();
        this.form()?.reset();
        this.translatorService.created.set(false); // reset the flag so effect won't fire again
      }
    });
  }
  onSubmit(form: NgForm) {
    let author: CreateTranslatorRequest = {
      name: form.value.name,
      description: form.value.description,
    };
    this.translatorService.create(author);
  }
}
