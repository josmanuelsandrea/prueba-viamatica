import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SessionHistoryModalComponent } from './session-history-modal.component';

describe('SessionHistoryModalComponent', () => {
  let component: SessionHistoryModalComponent;
  let fixture: ComponentFixture<SessionHistoryModalComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SessionHistoryModalComponent]
    });
    fixture = TestBed.createComponent(SessionHistoryModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
