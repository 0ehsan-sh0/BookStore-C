import { TestBed } from '@angular/core/testing';

import { BookPublicServiceService } from './book-public.service';

describe('BookPublicServiceService', () => {
  let service: BookPublicServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BookPublicServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
