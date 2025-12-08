import { Component, Input } from '@angular/core';
import { BookAllData } from '../../../models/book';
import { ImageService } from '../../../services/image.service';

@Component({
  selector: 'app-invoice-items',
  standalone: false,
  templateUrl: './invoice-items.component.html',
  styleUrl: './invoice-items.component.css'
})
export class InvoiceItemsComponent {
  @Input() books: BookAllData[] = [];
  
  constructor( public imageService: ImageService) {}
}
