import { Component, viewChild } from '@angular/core';
import { UserPanelService } from '../../services/user/user-panel.service';
import { Address, AddressPaginationInfo } from '../../models/address';
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
  pagination: AddressPaginationInfo = {
    pageNumber: 1,
    pageSize: 10,
    totalCount: 0,
    totalPages: 1,
  };
  createAddressModal = viewChild<ModalComponent>('createAddress');
  updateAddressModal = viewChild<ModalComponent>('updateAddress');

  constructor(private userAddressService: UserAddressService) {
    // update addresses on init
    this.userAddressService.getUserAddresses(
      this.pagination.pageNumber,
      this.pagination.pageSize
    );
    // Subscribe to user addresses BehaviorSubject
    this.userAddressService.addresses.subscribe((addresses) => {
      this.addresses = addresses ?? [];
    });

    // Subscribe to address pagination BehaviorSubject
    this.userAddressService.addressPagination.subscribe((pagination) => {
      if (pagination) {
        this.pagination = pagination;
      }
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

  changePage(page: number) {
    if (page !== this.pagination.pageNumber) {
      this.userAddressService.getUserAddresses(page, this.pagination.pageSize);
    }
  }

  getPageArray(): number[] {
    const total = this.pagination.totalPages;
    const current = this.pagination.pageNumber;

    const pages: number[] = [];

    for (let i = 1; i <= total; i++) {
      if (i === 1 || i === total || (i >= current - 1 && i <= current + 1)) {
        pages.push(i);
      } else if (i === current - 2 || i === current + 2) {
        pages.push(-1); // use -1 as ellipsis
      }
    }
    console.log(pages);

    return [...new Set(pages)];
  }
}
