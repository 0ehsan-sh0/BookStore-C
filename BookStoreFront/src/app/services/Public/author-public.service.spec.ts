import { TestBed } from '@angular/core/testing';

import { AuthorPublicService } from './author-public.service';

describe('AuthorPublicService', () => {
  let service: AuthorPublicService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AuthorPublicService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
