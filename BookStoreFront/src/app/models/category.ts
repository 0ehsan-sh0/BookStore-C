export interface Category {
  id: number;
  name: string;
  url: string;
  mainCategoryId?: number;
  createdAt?: Date;
  updatedAt?: Date;
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

// update-category-request.model.ts
export interface UpdateCategoryRequest {
  name: string;
  url: string;
  mainCategoryId?: number | null;
}