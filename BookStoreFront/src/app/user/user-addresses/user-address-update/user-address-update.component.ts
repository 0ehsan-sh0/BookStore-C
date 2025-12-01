import { Component, effect, output, viewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Address, UpdateAddressRequest } from '../../../models/address';
import { UserAddressService } from '../../../services/user/user-address.service';

@Component({
  selector: 'app-user-address-update',
  standalone: false,
  templateUrl: './user-address-update.component.html',
  styleUrl: './user-address-update.component.css',
})
export class UserAddressUpdateComponent {
  updated = output();
  errors: string[] = [];
  form = viewChild<NgForm>('form');
  address: Address = {} as Address;

  constructor(private userAddressService: UserAddressService) {
    userAddressService.address.subscribe((address) => {
      this.address = address;
    });

    // reactively track errors
    effect(() => {
      this.errors = userAddressService.updateErrors();
    });

    // reactively track update
    effect(() => {
      const isUpdated = userAddressService.updated();

      if (isUpdated) {
        this.updated.emit();
        this.form()?.reset();
        userAddressService.updated.set(false); // reset the flag so effect won't fire again
      }
    });
  }

  onSubmit(form: NgForm) {
    let address: UpdateAddressRequest = {
      name: form.value.name,
      lastName: form.value.lastName,
      address: form.value.address,
      city: form.value.city,
      state: form.value.state,
      postCode: form.value.postCode,
      phone: form.value.phone,
    };
    this.userAddressService.update(address, this.address.id);
  }
}
