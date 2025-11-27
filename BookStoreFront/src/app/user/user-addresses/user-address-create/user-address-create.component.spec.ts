import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserAddressCreateComponent } from './user-address-create.component';

describe('UserAddressCreateComponent', () => {
  let component: UserAddressCreateComponent;
  let fixture: ComponentFixture<UserAddressCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [UserAddressCreateComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserAddressCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
