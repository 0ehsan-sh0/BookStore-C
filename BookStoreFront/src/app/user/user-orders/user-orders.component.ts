import { Component } from '@angular/core';
import { Invoice } from '../../models/invoice';
import { UserPanelService } from '../../services/user/user-panel.service';

@Component({
  selector: 'app-user-orders',
  standalone: false,
  templateUrl: './user-orders.component.html',
  styleUrl: './user-orders.component.css',
})
export class UserOrdersComponent {
  invoices: Invoice[] = [];

  constructor(private userPanelService: UserPanelService) {
    // update invoices on init
    this.userPanelService.getUserInvoices();
    // Subscribe to user BehaviorSubject
    this.userPanelService.invoices.subscribe((invoices) => {
      this.invoices = invoices ?? [];
    });
  }
}
