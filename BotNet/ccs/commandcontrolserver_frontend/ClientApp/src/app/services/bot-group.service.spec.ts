import { TestBed } from '@angular/core/testing';

import { BotGroupService } from './bot-group.service';

describe('BotGroupService', () => {
  let service: BotGroupService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BotGroupService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
