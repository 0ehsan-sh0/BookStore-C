import { Component, effect, output, viewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { TranslatorService } from '../../../services/admin/translator.service';
import { Translator, UpdateTranslatorRequest } from '../../../models/translator';

@Component({
  selector: 'app-update-translator',
  standalone: false,
  templateUrl: './update-translator.component.html',
  styleUrl: './update-translator.component.css',
})
export class UpdateTranslatorComponent {
  updated = output();
  errors: string[] = [];
  form = viewChild<NgForm>('form');
  translator: Translator = {} as Translator;

  constructor(private translatorService: TranslatorService) {
    this.translatorService.translator.subscribe((translator) => {
      this.translator = translator;
    });
    // reactively track errors
    effect(() => {
      this.errors = this.translatorService.updateErrors();
    });

    // reactively track creation
    effect(() => {
      const isUpdated = this.translatorService.updated();

      if (isUpdated) {
        this.updated.emit();
        this.form()?.reset();
        this.translatorService.updated.set(false); // reset the flag so effect won't fire again
      }
    });
  }
  onSubmit(form: NgForm) {
    let author: UpdateTranslatorRequest = {
      name: form.value.name,
      description: form.value.description,
    };
    this.translatorService.update(author, this.translator.id);
  }
}
