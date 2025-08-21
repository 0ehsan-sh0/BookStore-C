import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BookTagComponent } from './book-tag.component';

describe('BookTagComponent', () => {
  let component: BookTagComponent;
  let fixture: ComponentFixture<BookTagComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [BookTagComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BookTagComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
