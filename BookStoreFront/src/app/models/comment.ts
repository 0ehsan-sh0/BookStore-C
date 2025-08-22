export interface Comment {
  id: number;
  comment: string;
  status: boolean;
  foreignTable: string;
  foreignId: number;
  createdAt: Date;
  updatedAt: Date;
}

export interface CommentListResponse {
  comments: Comment[];
  pagination: COPaginationInfo;
}

export interface COPaginationInfo {
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
}