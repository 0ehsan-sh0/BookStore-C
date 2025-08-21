export interface Tag {
  id: number;
  name: string;
  url: string;
  createdAt: Date;
  updatedAt: Date;
}

export interface TagListResponse {
  tags: Tag[];
  pagination: TagPaginationInfo;
}

export interface CreateTagRequest {
  name: string;
  url: string;
}

export interface UpdateTagRequest {
  name: string;
  url: string;
}

export interface TagPaginationInfo {
  totalCount: number;
  pageSize: number;
  pageNumber: number;
  totalPages: number;
}
