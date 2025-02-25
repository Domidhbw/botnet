import { Component, OnInit } from '@angular/core';
import { BotGroupService } from '../../services/bot-group.service';
import { BotGroup } from '../../models/bot-group.model';
import { BotManagementService } from '../../services/bot-management.service';

@Component({
  selector: 'app-group-list',
  templateUrl: './group-list.component.html',
  styleUrls: ['./group-list.component.scss']
})
export class GroupListComponent implements OnInit {
  groups: BotGroup[] = [];

  constructor(
    private groupService: BotGroupService,
    private botManagement: BotManagementService
  ) {}

  ngOnInit(): void {
    this.groupService.getGroups().subscribe(data => {
      this.groups = data;
    });
  }

  selectGroupBots(group: BotGroup) {
    this.botManagement.addBots(group.bots);
  }
}
