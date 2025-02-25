import { Component, OnInit } from '@angular/core';
import { BotService } from '../../services/bot.service';
import { Bot } from '../../models/bot.model';
import { BotManagementService } from '../../services/bot-management.service';

@Component({
  selector: 'app-bot-list',
  templateUrl: './bot-list.component.html',
  styleUrls: ['./bot-list.component.scss']
})
export class BotListComponent implements OnInit {
  bots: Bot[] = [];

  constructor(
    private botService: BotService,
    private botManagement: BotManagementService
  ) {}

  ngOnInit(): void {
    this.botService.getBots().subscribe(data => {
      this.bots = data;
    });
  }

  selectBot(bot: Bot) {
    this.botManagement.addBot(bot);
  }
}
