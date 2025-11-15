import { Address } from "./address";

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
}

export enum UserRole {
  Admin = 1,
  User = 2,
  // add the rest if they exist
}