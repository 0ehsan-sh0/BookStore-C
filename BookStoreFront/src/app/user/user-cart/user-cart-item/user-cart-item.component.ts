import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Book, BookAllData } from '../../../models/book';
import { ImageService } from '../../../services/image.service';

@Component({
  selector: 'app-user-cart-item',
  standalone: false,
  templateUrl: './user-cart-item.component.html',
  styleUrl: './user-cart-item.component.css',
})
export class UserCartItemComponent {
  @Input() item!: BookAllData;

  @Output() quantityChanged = new EventEmitter<{
    id: number;
    quantity: number;
  }>();
  @Output() removed = new EventEmitter<number>();

  constructor(private imageService: ImageService) {}

  increase(item: BookAllData) {
    const newQuantity = item.quantity + 1;
    this.quantityChanged.emit({ id: item.id, quantity: newQuantity });
  }

  decrease(item: BookAllData) {
    if (item.quantity > 1) {
      const newQuantity = item.quantity - 1;
      this.quantityChanged.emit({ id: item.id, quantity: newQuantity });
    }
  }

  remove(id: number) {
    this.removed.emit(id);
  }

  getImage(book: BookAllData): string {
    const img = book.images?.[0];
    if (!img) return 'https://placehold.co/300x400?text=No+Image';

    return this.imageService.getUrl(img.relativePath, img.storedFileName);
  }
}
