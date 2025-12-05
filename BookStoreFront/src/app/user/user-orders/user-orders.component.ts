import { Component } from '@angular/core';
import { Invoice, InvoicePaginationInfo } from '../../models/invoice';
import { UserPanelService } from '../../services/user/user-panel.service';
import { BookAllData } from '../../models/book';
import { ImageService } from '../../services/image.service';

@Component({
  selector: 'app-user-orders',
  standalone: false,
  templateUrl: './user-orders.component.html',
  styleUrl: './user-orders.component.css',
})
export class UserOrdersComponent {
  invoices: Invoice[] = [];
  pagination: InvoicePaginationInfo = {
    pageNumber: 1,
    pageSize: 10,
    totalCount: 0,
    totalPages: 1,
  };

  constructor(
    private userPanelService: UserPanelService,
    private imageService: ImageService
  ) {
    // update invoices on init
    this.userPanelService.getUserInvoices(
      this.pagination.pageNumber,
      this.pagination.pageSize
    );
    // Subscribe to user BehaviorSubject
    this.userPanelService.invoices.subscribe((invoices) => {
      this.invoices = invoices ?? [];
    });
    // Subscribe to invoice pagination BehaviorSubject
    this.userPanelService.invoicePagination.subscribe((pagination) => {
      if (pagination) {
        this.pagination = pagination;
      }
    });
  }

  // Returns first book image URL or placeholder
  getBookImageUrl(book: BookAllData): string {
    const img = book.images?.[0];
    if (!img) return 'https://placehold.co/48x64?text=No+Image';
    return this.imageService.getUrl(img.relativePath, img.storedFileName);
  }

  changePage(page: number) {
    if (page !== this.pagination.pageNumber) {
      this.userPanelService.getUserInvoices(page, this.pagination.pageSize);
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
