import { Component, effect, output, viewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { UserCommentService } from '../../../services/user/user-comment.service';
import { CreateCommentRequest } from '../../../models/comment';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-create-book-comment',
  standalone: false,
  templateUrl: './create-book-comment.component.html',
  styleUrl: './create-book-comment.component.css',
})
export class CreateBookCommentComponent {
  created = output();
  errors: string[] = [];
  form = viewChild<NgForm>('form');

  constructor(
    private userCommentService: UserCommentService,
    private route : ActivatedRoute) {
    // reactively track errors
    effect(() => {
      this.errors = this.userCommentService.createErrors();
    });

    // reactively track creation
    effect(() => {
      const isCreated = this.userCommentService.created();

      if (isCreated) {
        this.created.emit();
        this.form()?.reset();
        this.userCommentService.created.set(false); // reset the flag so effect won't fire again
      }
    });
  }

  onSubmit(form: NgForm) {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      const id = Number(idParam);
      let comment: CreateCommentRequest = {
        comment: form.value.comment,
      };
      this.userCommentService.create(id, comment);
    }
  }
}
