import { Component } from '@angular/core';
import { UserPanelService } from '../../services/user/user-panel.service';
import { ActivatedRoute } from '@angular/router';
import { Invoice } from '../../models/invoice';
import { PaymentStatus } from '../../models/enum';
import { AlertService } from '../../ui-service/alert.service';

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
    private panelService: UserPanelService,
    private alertService: AlertService
  ) {}

  ngOnInit(): void {
    // 1. Handle query parameters (status and message from redirect)
    this.route.queryParams.subscribe((params) => {
      const status = params['status'];
      const message = params['message'];

      if (status === 'success') {
        this.alertService.show(
          message || 'پرداخت با موفقیت انجام شد',
          'success'
        );
      } else if (status === 'failed') {
        this.alertService.show(message || 'پرداخت ناموفق بود', 'error');
      }
    });

    // 2. Get the 'id' parameter from the URL
    const invoiceId = this.route.snapshot.paramMap.get('id');

    if (invoiceId) {
      // 3. Call the method from your existing service
      this.panelService.getUserInvoice(+invoiceId);

      // 4. Subscribe to the 'userInvoice' BehaviorSubject from that service
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
