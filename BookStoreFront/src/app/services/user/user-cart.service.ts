import { Injectable, signal, inject, DestroyRef } from '@angular/core';
import { BookAllData } from '../../models/book';
import { CartSummary } from '../../models/user';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { Address } from '../../models/address';
import { HttpClient } from '@angular/common/http';
import { ErrorHandlerService } from '../error-handler.service';
import { ApiResponse } from '../../models/apiResponse';
import { CreateInvoiceRequest, PurchaseResponse } from '../../models/invoice';
import { toObservable, takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Injectable({
  providedIn: 'root',
})
export class UserCartService {
  private readonly apiUrl = 'api/user/purchase';
  private http = inject(HttpClient);
  private errorHandler = inject(ErrorHandlerService);
  private destroyRef = inject(DestroyRef);

  private cartStorageKey = 'ketabkade_cart_books';
  private addressStorageKey = 'ketabkade_cart_address';
  private cart: BookAllData[] = [];

  private _itemCountSig = signal<number>(0);
  itemCount = this._itemCountSig.asReadonly();
  itemCount$ = toObservable(this._itemCountSig);

  constructor() {
    this.loadCart();
  }

  private updateItemCount() {
    const count = this.cart.reduce((total, item) => total + item.quantity, 0);
    this._itemCountSig.set(count);
  }

  purchase(): Observable<ApiResponse<PurchaseResponse>> {
    const address = this.getAddress();
    const cartItems = this.getCart();

    if (!address || !address.id) {
      return throwError(() => new Error('No address selected.'));
    }
    if (!cartItems || cartItems.length === 0) {
      return throwError(() => new Error('Cart is empty.'));
    }

    const payload: CreateInvoiceRequest = {
      addressId: address.id,
      books: cartItems.map((item) => item.id),
      counts: cartItems.map((item) => item.quantity),
    };

    return this.http
      .post<ApiResponse<PurchaseResponse>>(this.apiUrl, payload)
      .pipe(
        tap((response) => {
          if (response.data) {
            this.clearCart();
            this.clearAddress();
          }
        }),
        catchError((err) => {
          this.errorHandler.handleError(err);
          return throwError(() => err);
        }),
        takeUntilDestroyed(this.destroyRef)
      );
  }

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

  private saveCart() {
    localStorage.setItem(this.cartStorageKey, JSON.stringify(this.cart));
    this.updateItemCount();
  }

  getCart(): BookAllData[] {
    return [...this.cart];
  }

  addToCart(book: BookAllData) {
    const existing = this.cart.find((x) => x.id === book.id);

    if (existing) {
      existing.quantity += book.quantity ?? 1;
    } else {
      this.cart.push({ ...book, quantity: book.quantity ?? 1 });
    }

    this.saveCart();
  }

  updateQuantity(id: number, quantity: number) {
    const item = this.cart.find((x) => x.id === id);

    if (item) {
      item.quantity = Math.max(1, quantity);
      this.saveCart();
    }
  }

  remove(id: number) {
    this.cart = this.cart.filter((item) => item.id !== id);
    this.saveCart();
  }

  clearCart() {
    this.cart = [];
    localStorage.removeItem(this.cartStorageKey);
    this.updateItemCount();
  }

  getTotal(): number {
    return this.cart.reduce((sum, item) => sum + item.price * item.quantity, 0);
  }

  getItemCount(): number {
    return this.cart.reduce((count, item) => count + item.quantity, 0);
  }

  getSummary(): CartSummary {
    const totalPrice = this.cart.reduce(
      (sum, item) => sum + item.price * item.quantity,
      0
    );
    const tax = totalPrice * 0.09;
    const discount = 0;
    const finalPrice = totalPrice + tax - discount;

    return {
      totalPrice,
      tax,
      discount,
      finalPrice,
    };
  }

  saveAddress(address: Address): void {
    try {
      localStorage.setItem(this.addressStorageKey, JSON.stringify(address));
    } catch (e) {
      console.error('Error saving address to local storage:', e);
    }
  }

  getAddress(): Address | null {
    try {
      const data = localStorage.getItem(this.addressStorageKey);
      return data ? JSON.parse(data) : null;
    } catch (e) {
      console.error('Error loading address from local storage:', e);
      return null;
    }
  }

  clearAddress(): void {
    try {
      localStorage.removeItem(this.addressStorageKey);
    } catch (e) {
      console.error('Error clearing address from local storage:', e);
    }
  }
}
