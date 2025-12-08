import { Component, viewChild } from '@angular/core';
import { InvoiceService } from '../../services/admin/invoice.service';
import { Invoice, InvoicePaginationInfo } from '../../models/invoice';
import { InvoiceStatus, PaymentStatus } from '../../models/enum';
import { ModalComponent } from '../../ui-service/modal/modal.component';

@Component({
  selector: 'app-invoice',
  standalone: false,
  templateUrl: './invoice.component.html',
  styleUrl: './invoice.component.css',
})
export class InvoiceComponent {
  invoices: Invoice[] = [];
  invoiceBooksModal = viewChild<ModalComponent>('invoiceBooks');

  pagination: InvoicePaginationInfo = {
    pageNumber: 1,
    pageSize: 20,
    totalCount: 0,
    totalPages: 1,
  };

  searchText: string = '';
  selectedBooks: any[] = [];

  constructor(private invoiceService: InvoiceService) {
    this.invoiceService.invoices.subscribe((data) => {
      this.invoices = data;
    });

    this.invoiceService.pagination.subscribe((p) => {
      this.pagination = p;
    });
  }

  ngOnInit() {
    this.invoiceService.getInvoices(
      this.pagination.pageNumber,
      this.pagination.pageSize
    );
  }

  onSearch() {
    this.invoiceService.getInvoices(1, 20, this.searchText);
  }

  changePage(page: number) {
    if (page !== this.pagination.pageNumber) {
      this.invoiceService.getInvoices(page, this.pagination.pageSize);
    }
  }

  // Similar pagination to your author component
  getPageArray(): number[] {
    const total = this.pagination.totalPages;
    const current = this.pagination.pageNumber;

    const pages: number[] = [];

    for (let i = 1; i <= total; i++) {
      if (i === 1 || i === total || (i >= current - 1 && i <= current + 1)) {
        pages.push(i);
      } else if (i === current - 2 || i === current + 2) {
        pages.push(-1);
      }
    }

    return [...new Set(pages)];
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

  getInvoiceStatusText(status: InvoiceStatus): string {
    switch (status) {
      case InvoiceStatus.Confirmed:
        return 'تایید شده';
      case InvoiceStatus.Pending:
        return 'در انتظار تایید';
      case InvoiceStatus.Rejected:
        return 'رد شده';
      default:
        return 'نامشخص';
    }
  }

  showInvoiceBooks(invoice: Invoice) {
    this.selectedBooks = invoice.books ?? [];
    this.invoiceBooksModal()!.open();
  }
}
