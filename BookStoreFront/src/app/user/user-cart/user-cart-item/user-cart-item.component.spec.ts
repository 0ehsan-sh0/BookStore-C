import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserCartItemComponent } from './user-cart-item.component';

describe('UserCartItemComponent', () => {
  let component: UserCartItemComponent;
  let fixture: ComponentFixture<UserCartItemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [UserCartItemComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserCartItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
