import { Component } from '@angular/core';
import { UserPanelService } from '../../services/user/user-panel.service';
import { Address } from '../../models/address';

@Component({
  selector: 'app-user-addresses',
  standalone: false,
  templateUrl: './user-addresses.component.html',
  styleUrl: './user-addresses.component.css',
})
export class UserAddressesComponent {
  addresses: Address[] = [];

  constructor(private userPanelService: UserPanelService) {
    // update addresses on init
    this.userPanelService.getUserAddresses();
    // Subscribe to user BehaviorSubject
    this.userPanelService.addresses.subscribe((addresses) => {
      this.addresses = addresses ?? [];
    });
  }


}
