import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserAddressUpdateComponent } from './user-address-update.component';

describe('UserAddressUpdateComponent', () => {
  let component: UserAddressUpdateComponent;
  let fixture: ComponentFixture<UserAddressUpdateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [UserAddressUpdateComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserAddressUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
