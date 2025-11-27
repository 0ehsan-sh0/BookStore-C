import { Component, effect, output, viewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { UserAddressService } from '../../../services/user/user-address.service';
import { CreateAddressRequest } from '../../../models/address';

@Component({
  selector: 'app-user-address-create',
  standalone: false,
  templateUrl: './user-address-create.component.html',
  styleUrl: './user-address-create.component.css',
})
export class UserAddressCreateComponent {
  created = output(); // emit event after successful creation
  errors: string[] = [];
  form = viewChild<NgForm>('form');

  constructor(private addressService: UserAddressService) {
    // track errors from error handler
    effect(() => {
      this.errors = this.addressService.createErrors();
    });

    // track created address success
    effect(() => {
      if (this.addressService.created()) {
        
        this.created.emit(); // inform parent
        this.form()?.reset(); // reset form
        this.addressService.created.set(false); // reset flag
      }
    });
  }

  onSubmit(form: NgForm) {
    const address: CreateAddressRequest = {
      name: form.value.name,
      lastName: form.value.lastName,
      phone: form.value.phone,
      postCode: form.value.postCode,
      state: form.value.state,
      city: form.value.city,
      address: form.value.address,
    };

    this.addressService.create(address);
  }
}
