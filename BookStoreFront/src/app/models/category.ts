export interface Category {
  id: number;
  name: string;
  url: string;
  mainCategoryId?: number;
  createdAt?: Date;
  updatedAt?: Date;
}