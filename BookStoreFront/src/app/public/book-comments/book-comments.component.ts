import { Component } from '@angular/core';
import { Comment, COPaginationInfo } from '../../models/comment';
import { CommentPublicService } from '../../services/Public/comment-public.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-book-comments',
  standalone: false,
  templateUrl: './book-comments.component.html',
  styleUrl: './book-comments.component.css',
})
export class BookCommentsComponent {
  comments: Comment[] = [];
  pagination: COPaginationInfo = {
    pageNumber: 1,
    pageSize: 10,
    totalCount: 0,
    totalPages: 1,
  };
  commentsLoading = true;

  constructor(
    private commentService: CommentPublicService,
    private route: ActivatedRoute
  ) {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      const id = Number(idParam);

      // Load comments
      this.commentsLoading = true;
      this.commentService.getBookComments(
        id,
        this.pagination.pageNumber,
        this.pagination.pageSize
      );

      this.commentService.comments.subscribe((c) => {
        this.comments = c;
        this.commentsLoading = false;
      });

      this.commentService.pagination.subscribe((p) => {
        this.pagination = p;
      });
    }
  }

  changePage(page: number) {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      const id = Number(idParam);
      if (page !== this.pagination.pageNumber) {
        this.commentService.getBookComments(id, page, this.pagination.pageSize);
      }
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
        pages.push(-1); // use -1 as ellipsis
      }
    }
    console.log(pages);

    return [...new Set(pages)];
  }
}
