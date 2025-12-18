import { Component, effect, output, viewChild, inject } from '@angular/core';
import { NgForm } from '@angular/forms';
import { CreateUserRequest, UserRole } from '../../../models/user';
import { UserService } from '../../../services/admin/user.service';

@Component({
  selector: 'app-create-user',
  standalone: false,
  templateUrl: './create-user.component.html',
  styleUrl: './create-user.component.css',
})
export class CreateUserComponent {
  created = output();
  errors: string[] = [];
  form = viewChild<NgForm>('form');
  userRole = UserRole; // Expose enum to the template

  private userService = inject(UserService);

  constructor() {
    // Reactively track errors
    effect(() => {
      this.errors = this.userService.createErrors();
    });

    // Reactively track creation
    effect(() => {
      const isCreated = this.userService.created();
      if (isCreated) {
        this.created.emit();
        this.form()?.reset();
        this.userService.created.set(false); // Reset the flag
      }
    });
  }

  onSubmit(form: NgForm) {
    if (form.invalid) {
      return;
    }

    const user: CreateUserRequest = {
      name: form.value.name,
      lastName: form.value.lastName,
      mobile: form.value.mobile,
      password: form.value.password,
      role: Number(form.value.role),
    };
    this.userService.create(user);
  }
}
