import { Component, viewChild } from '@angular/core';
import { Comment, COPaginationInfo } from '../../models/comment';
import { ModalComponent } from '../../ui-service/modal/modal.component';
import { CommentService } from '../../services/admin/comment.service';

@Component({
  selector: 'app-comment',
  standalone: false,
  templateUrl: './comment.component.html',
  styleUrl: './comment.component.css',
})
export class CommentComponent {
  comments: Comment[] = [];
  comment: Comment = {} as Comment;
  deleteId: number = 0;
  pagination: COPaginationInfo = {
    pageNumber: 1,
    pageSize: 20,
    totalCount: 0,
    totalPages: 1,
  };

  deleteCommentModal = viewChild<ModalComponent>('deleteComment');

  constructor(public commentService: CommentService) {
    this.commentService.comments.subscribe((comments) => {
      this.comments = comments;
    });
    this.commentService.pagination.subscribe((pagination) => {
      this.pagination = pagination;
    });

    this.commentService.getComments(
      this.pagination.pageNumber,
      this.pagination.pageSize
    );
  }

  changeStatus(id: number) {
    this.commentService.ChangeStatus(id);
  }

  delete(id: number) {
    this.deleteId = id;
    this.deleteCommentModal()!.open();
  }

  deleteConfirmed() {
    this.commentService.delete(this.deleteId);
    this.closeDialog('deleteCommentModal');
  }

  changePage(page: number) {
    if (page !== this.pagination.pageNumber) {
      this.commentService.getComments(page, this.pagination.pageSize);
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
    (pages);

    return [...new Set(pages)];
  }

  closeDialog(tab: string) {
    switch (tab) {
      case 'deleteCommentModal':
        this.deleteCommentModal()!.close();
        break;
    }
  }
}
