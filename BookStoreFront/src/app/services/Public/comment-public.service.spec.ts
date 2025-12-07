import { TestBed } from '@angular/core/testing';

import { CommentPublicService } from './comment-public.service';

describe('CommentPublicService', () => {
  let service: CommentPublicService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CommentPublicService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
