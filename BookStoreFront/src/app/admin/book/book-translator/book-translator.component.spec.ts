import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BookTranslatorComponent } from './book-translator.component';

describe('BookTranslatorComponent', () => {
  let component: BookTranslatorComponent;
  let fixture: ComponentFixture<BookTranslatorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [BookTranslatorComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BookTranslatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
