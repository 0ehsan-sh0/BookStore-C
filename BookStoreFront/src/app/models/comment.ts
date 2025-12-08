import { User } from './user';

export interface Comment {
  id: number;
  comment: string;
  status: boolean;
  foreignTable: string;
  foreignId: number;
  createdAt: Date;
  updatedAt: Date;
  userId: number;
  user?: User;
}

export interface CreateCommentRequest {
  comment: string;
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