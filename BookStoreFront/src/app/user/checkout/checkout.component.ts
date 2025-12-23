import { Component, viewChild } from '@angular/core';
import { UserAddressService } from '../../services/user/user-address.service';
import { UserCartService } from '../../services/user/user-cart.service';
import { Router } from '@angular/router';
import { AlertService } from '../../ui-service/alert.service';
import { Address } from '../../models/address';
import { ModalComponent } from '../../ui-service/modal/modal.component';

@Component({
  selector: 'app-checkout',
  standalone: false,
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.css',
})
export class CheckoutComponent {
  addresses: Address[] = [];
  selectedAddress: Address | null = null;
  isPurchasing = false;
  createAddressModal = viewChild<ModalComponent>('createAddress');

  constructor(
    public addressService: UserAddressService, // Made public to access in template
    private cartService: UserCartService,
    private router: Router,
    private alertService: AlertService
  ) {}

  ngOnInit(): void {
    // 1. Fetch user addresses from the API
    this.addressService.getUserAddresses();

    // 2. Subscribe to the addresses BehaviorSubject
    this.addressService.addresses.subscribe((addrs) => {
      this.addresses = addrs;

      // 3. Pre-select the first address if available
      if (addrs.length > 0) {
        this.selectedAddress = addrs[0];
      }
    });
  }

  // Set the selected address when a user chooses one
  selectAddress(address: Address): void {
    this.selectedAddress = address;
  }

  // Handle the final purchase action
  purchase(): void {
    // 1. Check if an address has been selected
    if (!this.selectedAddress) {
      this.alertService.show('لطفا یک آدرس را انتخاب کنید', 'error');
      return;
    }

    this.isPurchasing = true; // 2. Set loading state to true

    // 3. First, save the selected address to be used by the service
    this.cartService.saveAddress(this.selectedAddress);

    // 4. Call the purchase method from the service
    this.cartService.purchase().subscribe({
      next: (response) => {
        // 5. On success...
        this.isPurchasing = false; // Stop loading

        if (response.data?.paymentUrl) {
          this.alertService.show(
            response.message || 'در حال انتقال به درگاه پرداخت...',
            'success'
          );
          // Redirect the user to ZarinPal
          window.location.href = response.data.paymentUrl;
        } else {
          this.alertService.show('خطا در دریافت لینک پرداخت', 'error');
        }
      },
      error: (err) => {
        // 6. On error...
        this.isPurchasing = false; // Stop loading
      },
    });
  }

  closeDialog(tab: string) {
    switch (tab) {
      case 'createAddressModal':
        this.createAddressModal()!.close();
        break;
    }
  }

  createAddressModalOpen() {
    this.createAddressModal()!.open();
  }
}
