import { Book, BookListResponse } from "./book";

export interface Category {
  id: number;
  name: string;
  url: string;
  mainCategoryId?: number;
  createdAt?: Date;
  updatedAt?: Date;
  subCategories?: Category[];
  books?: Book[];
}

export interface CPaginationInfo {
  totalCount: number;
  pageSize: number;
  pageNumber: number;
  totalPages: number;
}

export interface CategoryListResponse {
  categories?: Category[];
  pagination?: CPaginationInfo;
}

// create-category-request.model.ts
export interface CreateCategoryRequest {
  name: string;
  url: string;
  mainCategoryId?: number | null;
}

export interface CategoryDetails {
  category: Category;
  books: BookListResponse;
}

// update-category-request.model.ts
export interface UpdateCategoryRequest {
  name: string;
  url: string;
  mainCategoryId?: number | null;
}
