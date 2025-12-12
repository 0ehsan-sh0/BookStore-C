import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { userPanelGuard } from './user-panel.guard';

describe('userPanelGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => userPanelGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
