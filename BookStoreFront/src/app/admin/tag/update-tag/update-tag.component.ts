import { Component, effect, output, viewChild } from '@angular/core';
import { Tag, UpdateTagRequest } from '../../../models/tag';
import { NgForm } from '@angular/forms';
import { TagService } from '../../../services/admin/tag.service';

@Component({
  selector: 'app-update-tag',
  standalone: false,
  templateUrl: './update-tag.component.html',
  styleUrl: './update-tag.component.css',
})
export class UpdateTagComponent {
  updated = output();
  errors: string[] = [];
  form = viewChild<NgForm>('form');
  tag: Tag = {} as Tag;

  constructor(private tagService: TagService) {
    this.tagService.tag.subscribe((tag) => {
      this.tag = tag;
    });
    // reactively track errors
    effect(() => {
      this.errors = this.tagService.updateErrors();
    });

    // reactively track creation
    effect(() => {
      const isUpdated = this.tagService.updated();

      if (isUpdated) {
        this.updated.emit();
        this.form()?.reset();
        this.tagService.updated.set(false); // reset the flag so effect won't fire again
      }
    });
  }
  onSubmit(form: NgForm) {
    let category: UpdateTagRequest = {
      name: form.value.name,
      url: form.value.url,
    };
    this.tagService.update(category, this.tag.id);
  }
}
