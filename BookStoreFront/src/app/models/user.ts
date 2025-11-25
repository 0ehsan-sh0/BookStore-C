import { Address } from "./address";
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
  addresses?: Address[];
  invoices?: Invoice[];
  wishList?: BookAllData[];
}

export enum UserRole {
  Admin = 1,
  User = 2,
  // add the rest if they exist
}