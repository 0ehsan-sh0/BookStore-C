import { Component } from '@angular/core';
import { Invoice } from '../../models/invoice';
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

  constructor(private userPanelService: UserPanelService, private imageService: ImageService) {
    // update invoices on init
    this.userPanelService.getUserInvoices();
    // Subscribe to user BehaviorSubject
    this.userPanelService.invoices.subscribe((invoices) => {
      this.invoices = invoices ?? [];
    });
  }

  // Returns first book image URL or placeholder
  getBookImageUrl(book: BookAllData): string {
    const img = book.images?.[0];  
    if (!img) return 'https://placehold.co/48x64?text=No+Image';
    return this.imageService.getUrl(img.relativePath, img.storedFileName);
  }
}
