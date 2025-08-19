export interface Author {
  id: number;
  name: string;
  description: string;
  createdAt?: Date;
  updatedAt?: Date;
}

export interface APaginationInfo {
  totalCount: number;
  pageSize: number;
  pageNumber: number;
  totalPages: number;
}

export interface AuthorListResponse {
  authors?: Author[];
  pagination?: APaginationInfo;
}

// create-author-request.ts
export interface CreateAuthorRequest {
  name: string;
  description: string;
}

// update-author-request.ts
export interface UpdateAuthorRequest {
  name: string;
  description: string;
}