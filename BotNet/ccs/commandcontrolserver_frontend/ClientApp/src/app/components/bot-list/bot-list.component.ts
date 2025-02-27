import { Component, OnInit } from '@angular/core';
import { BotService } from '../../services/bot.service';
import { BotManagementService } from '../../services/bot-management.service';
import { Bot } from '../../models/bot.model';

@Component({
  selector: 'app-bot-list',
  templateUrl: './bot-list.component.html',
  styleUrls: ['./bot-list.component.scss']
})
export class BotListComponent implements OnInit {
  bots: Bot[] = [];
  selectedBotIds: string[] = [];

  constructor(
    private botService: BotService,
    private botManagement: BotManagementService
  ) {}

  ngOnInit(): void {
    this.botService.getBots().subscribe(data => {
      this.bots = data;
    });
  }

  isSelected(bot: Bot): boolean {
    return this.selectedBotIds.includes(bot.botId.toString());
  }

  toggleBotSelection(bot: Bot): void {
    const botIdStr = bot.botId.toString();
    const idx = this.selectedBotIds.indexOf(botIdStr);
    if (idx === -1) {
      this.selectedBotIds.push(botIdStr);
      this.botManagement.addBot(bot);
    } else {
      this.selectedBotIds.splice(idx, 1);
      this.botManagement.removeBot(bot);
    }
  }

  reloadBots(): void {
    this.botService.getBots().subscribe(data => {
      this.bots = data;
    });
  }
 
}
