import { PaginationInfo } from "./apiResponse";

export interface Address {
  id: number;
  name: string;
  lastName: string;
  phone: string;
  postCode: string;
  state: string;
  city: string;
  address: string;
  userId: number;
  createdAt?: string | null;
  updatedAt?: string | null;
}

export interface AddressPaginationInfo extends PaginationInfo {}

export interface AddressListResponse {
  addresses?: Address[] | null;
  pagination?: AddressPaginationInfo | null;
}

export interface CreateAddressRequest {
  name: string;
  lastName: string;
  phone: string;
  postCode: string;
  state: string;
  city: string;
  address: string;
}

export interface UpdateAddressRequest {
  name: string;
  lastName: string;
  phone: string;
  postCode: string;
  state: string;
  city: string;
  address: string;
}