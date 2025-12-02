import { Injectable } from '@angular/core';
import { BookAllData } from '../../models/book';
import { CartSummary } from '../../models/user';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class UserCartService {
  private storageKey = 'user_cart';
  private cart: BookAllData[] = [];
  private _itemCount = new BehaviorSubject<number>(0);
  // Expose it as an Observable for components to subscribe to
  itemCount$: Observable<number> = this._itemCount.asObservable();

  constructor() {
    this.loadCart();
  }

  private updateItemCount() {
    const count = this.cart.reduce((total, item) => total + item.quantity, 0);
    this._itemCount.next(count); // Emit the new count
  }
  // ---------------------------------------------------
  // LOAD CART FROM LOCALSTORAGE
  // ---------------------------------------------------
  private loadCart() {
    try {
      const data = localStorage.getItem(this.storageKey);
      this.cart = data ? JSON.parse(data) : [];
      this.updateItemCount();
    } catch {
      this.cart = [];
      localStorage.removeItem(this.storageKey);
      this.updateItemCount();
    }
  }

  // ---------------------------------------------------
  // SAVE CART TO LOCALSTORAGE
  // ---------------------------------------------------
  private saveCart() {
    localStorage.setItem(this.storageKey, JSON.stringify(this.cart));
    this.updateItemCount();
  }

  // ---------------------------------------------------
  // GET ITEMS
  // ---------------------------------------------------
  getCart(): BookAllData[] {
    return [...this.cart]; // return clone
  }

  // ---------------------------------------------------
  // ADD BOOK TO CART
  // ---------------------------------------------------
  addToCart(book: BookAllData) {
    const existing = this.cart.find((x) => x.id === book.id);

    if (existing) {
      existing.quantity += book.quantity ?? 1;
    } else {
      this.cart.push({ ...book, quantity: book.quantity ?? 1 });
    }

    this.saveCart();
  }

  // ---------------------------------------------------
  // UPDATE QUANTITY
  // ---------------------------------------------------
  updateQuantity(id: number, quantity: number) {
    const item = this.cart.find((x) => x.id === id);

    if (item) {
      item.quantity = Math.max(1, quantity); // prevent 0
      this.saveCart();
    }
  }

  // ---------------------------------------------------
  // REMOVE ONE ITEM
  // ---------------------------------------------------
  remove(id: number) {
    this.cart = this.cart.filter((item) => item.id !== id);
    this.saveCart();
  }

  // ---------------------------------------------------
  // CLEAR ALL CART
  // ---------------------------------------------------
  clearCart() {
    this.cart = [];
    localStorage.removeItem(this.storageKey);
    this.updateItemCount();
  }

  // ---------------------------------------------------
  // GET TOTAL PRICE
  // ---------------------------------------------------
  getTotal(): number {
    return this.cart.reduce((sum, item) => sum + item.price * item.quantity, 0);
  }

  // ---------------------------------------------------
  // GET TOTAL ITEMS COUNT
  // ---------------------------------------------------
  getItemCount(): number {
    return this.cart.reduce((count, item) => count + item.quantity, 0);
  }

  // ---------------------------------------------------
  // GET CART SUMMARY
  // ---------------------------------------------------
  getSummary(): CartSummary {
    // 1. Calculate the base total price
    const totalPrice = this.cart.reduce(
      (sum, item) => sum + item.price * item.quantity,
      0
    );

    // 2. Calculate tax based on the total
    const tax = totalPrice * 0.09;

    // 3. Calculate discount
    const discount = 0;

    // 4. Calculate the final price
    const finalPrice = totalPrice + tax - discount;

    // 5. Return everything in a single object
    return {
      totalPrice,
      tax,
      discount,
      finalPrice,
    };
  }
}
