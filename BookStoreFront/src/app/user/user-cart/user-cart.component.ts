import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { BookAllData } from '../../models/book';
import { UserCartService } from '../../services/user/user-cart.service';

@Component({
  selector: 'app-user-cart',
  standalone: false,
  templateUrl: './user-cart.component.html',
  styleUrl: './user-cart.component.css',
})
export class UserCartComponent {
  cartItems: BookAllData[] = [];

  totalPrice = 0;
  discount = 0;
  finalPrice = 0;
  tax = 0;

  constructor(private cartService: UserCartService, private router: Router) {}

  ngOnInit(): void {
    this.loadCart();
  }

  // ------------------------------
  // LOAD CART FROM SERVICE
  // ------------------------------
  loadCart() {
    this.cartItems = this.cartService.getCart(); // returns BookAllData[]

    this.calculateSummary();
  }

  // ------------------------------
  // UPDATE QUANTITY FROM CHILD
  // ------------------------------
  updateQuantity(event: { id: number; quantity: number }) {
    this.cartService.updateQuantity(event.id, event.quantity);
    this.loadCart();
  }

  // ------------------------------
  // REMOVE ITEM FROM CHILD
  // ------------------------------
  removeFromCart(id: number) {
    this.cartService.remove(id);
    this.loadCart();
  }

  // ------------------------------
  // CALCULATE TOTALS
  // ------------------------------
  calculateSummary() {
    // Get all calculated values from the service
    const summary = this.cartService.getSummary();
    
    // Assign the values to the component's properties
    this.totalPrice = summary.totalPrice;
    this.tax = summary.tax;
    this.discount = summary.discount;
    this.finalPrice = summary.finalPrice;
  }

  // ------------------------------
  // HANDLE CHECKOUT BUTTON
  // ------------------------------
  goToCheckout() {
    this.router.navigate(['/checkout']);
  }
}
