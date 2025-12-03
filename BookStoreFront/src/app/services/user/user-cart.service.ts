import { Injectable } from '@angular/core';
import { BookAllData } from '../../models/book';
import { CartSummary } from '../../models/user';
import { BehaviorSubject, catchError, Observable, tap, throwError } from 'rxjs';
import { Address } from '../../models/address';
import { HttpClient } from '@angular/common/http';
import { ErrorHandlerService } from '../error-handler.service';
import { ApiResponse } from '../../models/apiResponse';
import { CreateInvoiceRequest } from '../../models/invoice';

@Injectable({
  providedIn: 'root',
})
export class UserCartService {
  private readonly apiUrl = 'api/user/purchase';
  private cartStorageKey = 'ketabkade_cart_books';
  private addressStorageKey = 'ketabkade_cart_address';
  private cart: BookAllData[] = [];
  private _itemCount = new BehaviorSubject<number>(0);
  // Expose it as an Observable for components to subscribe to
  itemCount$: Observable<number> = this._itemCount.asObservable();

  constructor(
    private http: HttpClient,
    private errorHandler: ErrorHandlerService
  ) {
    this.loadCart();
  }

  private updateItemCount() {
    const count = this.cart.reduce((total, item) => total + item.quantity, 0);
    this._itemCount.next(count); // Emit the new count
  }
  // ---------------------------------------------------
  // --- PURCHASE (CHECKOUT) ---
  // ---------------------------------------------------
  purchase(): Observable<ApiResponse<any>> {
    // 1. Get the required data from local storage and state
    const address = this.getAddress();
    const cartItems = this.getCart();

    // 2. Validate that we have an address and items
    if (!address || !address.id) {
      // Return an observable that emits an error immediately
      return throwError(() => new Error('No address selected.'));
    }
    if (!cartItems || cartItems.length === 0) {
      return throwError(() => new Error('Cart is empty.'));
    }

    // 3. Prepare the request payload
    const payload: CreateInvoiceRequest = {
      addressId: address.id,
      books: cartItems.map(item => item.id), // Create an array of book IDs
      counts: cartItems.map(item => item.quantity), // Create an array of quantities
    };

    // 4. Make the API call
    return this.http.post<ApiResponse<any>>(this.apiUrl, payload).pipe(
      tap((response) => {
        // This 'tap' operator runs on success BEFORE the subscriber in the component
        if (response.data) {
          // 5. Clear cart and address from local storage on successful purchase
          this.clearCart();
          this.clearAddress();
        }
      }),
      catchError(err => {
        // 6. Use the existing error handler for any API errors
        this.errorHandler.handleError(err);
        return throwError(() => err); // Re-throw the error for the component's error block
      })
    );
  }
  
  // ---------------------------------------------------
  // LOAD CART FROM LOCALSTORAGE
  // ---------------------------------------------------
  private loadCart() {
    try {
      const data = localStorage.getItem(this.cartStorageKey);
      this.cart = data ? JSON.parse(data) : [];
      this.updateItemCount();
    } catch {
      this.cart = [];
      localStorage.removeItem(this.cartStorageKey);
      this.updateItemCount();
    }
  }

  // ---------------------------------------------------
  // SAVE CART TO LOCALSTORAGE
  // ---------------------------------------------------
  private saveCart() {
    localStorage.setItem(this.cartStorageKey, JSON.stringify(this.cart));
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
    localStorage.removeItem(this.cartStorageKey);
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

  // --- ADDRESS MANAGEMENT ---
  // ---------------------------------------------------
  // SAVE ADDRESS TO LOCALSTORAGE
  // ---------------------------------------------------
  saveAddress(address: Address): void {
    try {
      localStorage.setItem(this.addressStorageKey, JSON.stringify(address));
    } catch (e) {
      console.error('Error saving address to local storage:', e);
    }
  }

  // ---------------------------------------------------
  // GET ADDRESS FROM LOCALSTORAGE
  // ---------------------------------------------------
  getAddress(): Address | null {
    try {
      const data = localStorage.getItem(this.addressStorageKey);
      return data ? JSON.parse(data) : null;
    } catch (e) {
      console.error('Error loading address from local storage:', e);
      return null;
    }
  }

  // ---------------------------------------------------
  // CLEAR SAVED ADDRESS
  // ---------------------------------------------------
  clearAddress(): void {
    try {
      localStorage.removeItem(this.addressStorageKey);
    } catch (e) {
      console.error('Error clearing address from local storage:', e);
    }
  }
}
