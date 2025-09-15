import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PublicSidebarComponent } from './public-sidebar.component';

describe('PublicSidebarComponent', () => {
  let component: PublicSidebarComponent;
  let fixture: ComponentFixture<PublicSidebarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PublicSidebarComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PublicSidebarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
