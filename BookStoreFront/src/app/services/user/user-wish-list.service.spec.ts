import { TestBed } from '@angular/core/testing';

import { UserWishListService } from './user-wish-list.service';

describe('UserWishListService', () => {
  let service: UserWishListService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UserWishListService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
