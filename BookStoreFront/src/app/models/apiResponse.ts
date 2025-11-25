import { InjectionToken } from '@angular/core';

export interface ApiResponse<T> {
  message: string;
  data?: T;        // present on success
  errors?: any;    // present on error
}

export const API_URL = new InjectionToken<string>('API_URL');

export interface PaginationInfo {
  totalCount: number | null;
  pageSize: number | null;
  pageNumber: number | null;
  totalPages: number | null;
}