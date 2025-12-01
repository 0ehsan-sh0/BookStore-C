import { Component, viewChild } from '@angular/core';
import { UserPanelService } from '../../services/user/user-panel.service';
import { Address } from '../../models/address';
import { ModalComponent } from '../../ui-service/modal/modal.component';
import { UserAddressService } from '../../services/user/user-address.service';

@Component({
  selector: 'app-user-addresses',
  standalone: false,
  templateUrl: './user-addresses.component.html',
  styleUrl: './user-addresses.component.css',
})
export class UserAddressesComponent {
  addresses: Address[] = [];
  createAddressModal = viewChild<ModalComponent>('createAddress');
  updateAddressModal = viewChild<ModalComponent>('updateAddress');

  constructor(private userAddressService: UserAddressService) {
    // update addresses on init
    this.userAddressService.getUserAddresses();
    // Subscribe to user BehaviorSubject
    this.userAddressService.addresses.subscribe((addresses) => {
      this.addresses = addresses ?? [];
    });
  }

  closeDialog(tab: string) {
    switch (tab) {
      case 'createAddressModal':
        this.createAddressModal()!.close();
        break;
      case 'updateAddressModal':
        this.updateAddressModal()!.close();
        break;
    }
  }

  create() {
    this.createAddressModal()!.open();
  }

  update(id: number) {
    this.userAddressService.getById(id);
    this.updateAddressModal()!.open();
  }
}
