import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BotEditModalComponent } from './bot-edit-modal.component';

describe('BotEditModalComponent', () => {
  let component: BotEditModalComponent;
  let fixture: ComponentFixture<BotEditModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BotEditModalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BotEditModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
