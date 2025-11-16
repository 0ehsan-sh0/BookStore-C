import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserPublicComponent } from './user-public.component';

describe('UserPublicComponent', () => {
  let component: UserPublicComponent;
  let fixture: ComponentFixture<UserPublicComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [UserPublicComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserPublicComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
