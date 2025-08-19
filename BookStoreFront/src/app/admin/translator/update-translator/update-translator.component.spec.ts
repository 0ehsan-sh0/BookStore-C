import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateTranslatorComponent } from './update-translator.component';

describe('UpdateTranslatorComponent', () => {
  let component: UpdateTranslatorComponent;
  let fixture: ComponentFixture<UpdateTranslatorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [UpdateTranslatorComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UpdateTranslatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
