import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PublicBannersComponent } from './public-banners.component';

describe('PublicBannersComponent', () => {
  let component: PublicBannersComponent;
  let fixture: ComponentFixture<PublicBannersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PublicBannersComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PublicBannersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
