import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateBookCommentComponent } from './create-book-comment.component';

describe('CreateBookCommentComponent', () => {
  let component: CreateBookCommentComponent;
  let fixture: ComponentFixture<CreateBookCommentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CreateBookCommentComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateBookCommentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
