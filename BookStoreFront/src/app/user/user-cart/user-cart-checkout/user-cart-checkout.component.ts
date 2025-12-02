import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-user-cart-checkout',
  standalone: false,
  templateUrl: './user-cart-checkout.component.html',
  styleUrl: './user-cart-checkout.component.css',
})
export class UserCartCheckoutComponent {
  @Input() totalPrice = 0;
  @Input() discount = 0;
  @Input() finalPrice = 0;
  @Input() tax = 0;

  @Output() checkoutClicked = new EventEmitter<void>();

  checkout() {
    this.checkoutClicked.emit();
  }
}
