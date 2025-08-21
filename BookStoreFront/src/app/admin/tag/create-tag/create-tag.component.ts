import { Component, effect, output, viewChild } from '@angular/core';
import { CreateTagRequest, Tag } from '../../../models/tag';
import { NgForm } from '@angular/forms';
import { TagService } from '../../../services/admin/tag.service';

@Component({
  selector: 'app-create-tag',
  standalone: false,
  templateUrl: './create-tag.component.html',
  styleUrl: './create-tag.component.css',
})
export class CreateTagComponent {
  created = output();
  errors: string[] = [];
  form = viewChild<NgForm>('form');

  constructor(private tagService: TagService) {
    // reactively track errors
    effect(() => {
      this.errors = this.tagService.createErrors();
    });

    // reactively track creation
    effect(() => {
      const isCreated = this.tagService.created();

      if (isCreated) {
        this.created.emit();
        this.form()?.reset();
        this.tagService.created.set(false); // reset the flag so effect won't fire again
      }
    });
  }
  onSubmit(form: NgForm) {
    let category: CreateTagRequest = {
      name: form.value.name,
      url: form.value.url,
    };
    this.tagService.create(category);
  }
}
