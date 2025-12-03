import { Component } from '@angular/core';
import { UserPanelService } from '../../services/user/user-panel.service';
import { ActivatedRoute } from '@angular/router';
import { Invoice } from '../../models/invoice';
import { PaymentStatus } from '../../models/enum';

@Component({
  selector: 'app-user-invoice',
  standalone: false,
  templateUrl: './user-invoice.component.html',
  styleUrl: './user-invoice.component.css',
})
export class UserInvoiceComponent {
  invoice: Invoice | null = null;
  isLoading = true;
  PaymentStatus = PaymentStatus;

  constructor(
    private route: ActivatedRoute,
    // Inject your existing UserPanelService
    private panelService: UserPanelService
  ) {}

  ngOnInit(): void {
    // Get the 'id' parameter from the URL
    const invoiceId = this.route.snapshot.paramMap.get('id');

    if (invoiceId) {
      // 1. Call the method from your existing service
      this.panelService.getUserInvoice(+invoiceId);

      // 2. Subscribe to the 'userInvoice' BehaviorSubject from that service
      this.panelService.userInvoice.subscribe((data) => {
        this.invoice = data;
        this.isLoading = false; // Stop loading once data is received (or null)
      });
    } else {
      console.error('No invoice ID found in URL.');
      this.isLoading = false;
    }
  }

   getPaymentStatusText(status: PaymentStatus): string {
    switch (status) {
      case PaymentStatus.Completed:
        return 'پرداخت موفق';
      case PaymentStatus.Failed:
        return 'ناموفق';
      case PaymentStatus.Initiated:
        return 'در انتظار پرداخت';
      default:
        return 'نامشخص';
    }
  }

  /**
   * Triggers the browser's print dialog.
   */
  print(): void {
    window.print();
  }
}
