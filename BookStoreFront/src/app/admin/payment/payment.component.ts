import { Component, inject } from '@angular/core';
import { PaymentService } from '../../services/admin/payment.service';
import { Payment, PaymentPaginationInfo } from '../../models/payment';

@Component({
  selector: 'app-payment',
  standalone: false,
  templateUrl: './payment.component.html',
  styleUrl: './payment.component.css',
})
export class PaymentComponent {
  paymentService = inject(PaymentService);
  payments = this.paymentService.payments;
  pagination = this.paymentService.pagination;
  searchText: string = '';

  constructor() {}

  ngOnInit() {
    this.loadPayments();
  }

  loadPayments(page: number = 1) {
    this.paymentService.getPayments(
      page,
      this.pagination().pageSize,
      this.searchText
    );
  }

  onSearch() {
    this.loadPayments(1);
  }

  changePage(page: number) {
    if (page !== this.pagination().pageNumber) {
      this.loadPayments(page);
    }
  }

  getPageArray(): number[] {
    const total = this.pagination().totalPages;
    const current = this.pagination().pageNumber;

    const pages: number[] = [];

    for (let i = 1; i <= total; i++) {
      if (i === 1 || i === total || (i >= current - 1 && i <= current + 1)) {
        pages.push(i);
      } else if (i === current - 2 || i === current + 2) {
        pages.push(-1); // ellipsis
      }
    }

    return [...new Set(pages)];
  }

  getStatusText(status: number): string {
    switch (status) {
      case 1:
        return 'پرداخت موفق';
      case 2:
        return 'ناموفق';
      case 3:
        return 'در انتظار پرداخت';
      default:
        return 'نامشخص';
    }
  }
}
