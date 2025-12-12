import { Component } from '@angular/core';
import { Comment, COPaginationInfo } from '../../models/comment';
import { UserCommentService } from '../../services/user/user-comment.service';

@Component({
  selector: 'app-user-comment',
  standalone: false,
  templateUrl: './user-comment.component.html',
  styleUrl: './user-comment.component.css',
})
export class UserCommentComponent {
  comments: Comment[] = [];

  pagination: COPaginationInfo = {
    pageNumber: 1,
    pageSize: 10,
    totalCount: 0,
    totalPages: 1,
  };

  constructor(private commentService: UserCommentService) {
    // Load initial list
    this.commentService.getUserComments(
      this.pagination.pageNumber,
      this.pagination.pageSize
    );

    // Listen for comments updates
    this.commentService.comments.subscribe((comments) => {
      this.comments = comments ?? [];
    });

    // Listen for pagination updates
    this.commentService.pagination.subscribe((p) => {
      this.pagination = p ?? this.pagination;
    });
  }

  // Pagination
  changePage(page: number) {
    if (page !== this.pagination.pageNumber) {
      this.commentService.getUserComments(page, this.pagination.pageSize);
    }
  }

  getPageArray(): number[] {
    const total = this.pagination.totalPages;
    const current = this.pagination.pageNumber;
    const pages: number[] = [];

    for (let i = 1; i <= total; i++) {
      if (i === 1 || i === total || (i >= current - 1 && i <= current + 1)) {
        pages.push(i);
      } else if (i === current - 2 || i === current + 2) {
        pages.push(-1);
      }
    }

    return [...new Set(pages)];
  }
}
