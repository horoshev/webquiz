import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { QuestionRandomComponent } from './question-random.component';

describe('QuestionRandomComponent', () => {
  let component: QuestionRandomComponent;
  let fixture: ComponentFixture<QuestionRandomComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ QuestionRandomComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QuestionRandomComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
