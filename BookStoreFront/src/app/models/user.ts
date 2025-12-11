import { Address } from "./address";
import { PaginationInfo } from "./apiResponse";
import { BookAllData } from "./book";
import { Invoice } from "./invoice";

export interface User {
  id: number;
  name: string;
  lastName: string;
  mobile: string;
  role: UserRole;
  createdAt?: string | null;
  updatedAt?: string | null;
  loggedInAt?: string | null;
  buyCount?: number;
  addresses?: Address[];
  invoices?: Invoice[];
  wishList?: BookAllData[];
}

export interface UserListResponse {
  users: User[];
  pagination: UPaginationInfo;
}

export interface UPaginationInfo extends PaginationInfo {}

export enum UserRole {
  Admin = 1,
  User = 2,
  // add the rest if they exist
}

export interface CartSummary {
  totalPrice: number;
  tax: number;
  discount: number;
  finalPrice: number;
}

export interface UpdateUserRequest {
  name: string;
  lastName: string;
  role: UserRole;
}

export interface CreateUserRequest {
  name: string;
  lastName: string;
  mobile: string;
  password: string;
  role: UserRole;
}