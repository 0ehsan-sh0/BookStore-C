import { Component, effect, output, viewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { UserService } from '../../../services/admin/user.service';
import { User, UpdateUserRequest, UserRole } from '../../../models/user';

@Component({
  selector: 'app-update-user',
  standalone: false,
  templateUrl: './update-user.component.html',
  styleUrl: './update-user.component.css'
})
export class UpdateUserComponent {
  updated = output();
  errors: string[] = [];
  form = viewChild<NgForm>('form');
  user: User = {} as User;
  userRole = UserRole; // Expose enum to the template

  constructor(private userService: UserService) {
    this.userService.user.subscribe((user) => {
      this.user = user;
    });

    // Reactively track errors
    effect(() => {
      this.errors = this.userService.updateErrors();
    });

    // Reactively track update
    effect(() => {
      const isUpdated = this.userService.updated();
      if (isUpdated) {
        this.updated.emit();
        this.form()?.reset();
        this.userService.updated.set(false); // Reset the flag
      }
    });
  }

  onSubmit(form: NgForm) {
    if (form.invalid) {
      return;
    }

    const user: UpdateUserRequest = {
      name: form.value.name,
      lastName: form.value.lastName,
      role: Number(form.value.role)
    };

    this.userService.update(user, this.user.id);
  }
}
