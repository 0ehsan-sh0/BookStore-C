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