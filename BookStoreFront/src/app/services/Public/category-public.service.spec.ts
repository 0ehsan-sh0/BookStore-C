import { TestBed } from '@angular/core/testing';

import { CategoryPublicService } from './category-public.service';

describe('CategoryPublicService', () => {
  let service: CategoryPublicService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CategoryPublicService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
