import { Component, OnInit, inject, DestroyRef, signal } from '@angular/core';
import { CommentPublicService } from '../../services/Public/comment-public.service';
import { ActivatedRoute } from '@angular/router';
import { ModalComponent } from '../../ui-service/modal/modal.component';
import { viewChild } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-book-comments',
  standalone: false,
  templateUrl: './book-comments.component.html',
  styleUrl: './book-comments.component.css',
})
export class BookCommentsComponent implements OnInit {
  private destroyRef = inject(DestroyRef);
  private route = inject(ActivatedRoute);
  public commentService = inject(CommentPublicService);

  commentsLoading = signal(true);
  createBookComment = viewChild<ModalComponent>('createComment');
  bookId!: number;

  ngOnInit(): void {
    this.route.paramMap
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((params) => {
        const id = Number(params.get('id'));
        if (id) {
          this.bookId = id;
          this.loadComments();
        }
      });
  }

  loadComments() {
    this.commentsLoading.set(true);
    this.commentService.getBookComments(this.bookId);
    // Assuming the signal update from service will be reactive in template
    // We could use an effect or just trust the signal
    this.commentsLoading.set(false); // Simplified, ideally service would expose loading
  }

  create() {
    this.createBookComment()?.open();
  }

  changePage(page: number) {
    const pagination = this.commentService.pagination();
    if (page !== pagination.pageNumber) {
      this.commentService.getBookComments(
        this.bookId,
        page,
        pagination.pageSize
      );
    }
  }

  getPageArray(): number[] {
    const pagination = this.commentService.pagination();
    const total = pagination.totalPages;
    const current = pagination.pageNumber;

    const pages: number[] = [];

    for (let i = 1; i <= total; i++) {
      if (i === 1 || i === total || (i >= current - 1 && i <= current + 1)) {
        pages.push(i);
      } else if (i === current - 2 || i === current + 2) {
        pages.push(-1); // use -1 as ellipsis
      }
    }

    return [...new Set(pages)];
  }

  closeDialog(tab: string) {
    if (tab === 'createBookCommentModal') {
      this.createBookComment()?.close();
    }
  }
}
