import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
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

  toggleBotSelection(bot: Bot, event: Event): void {
    const inputElement = event.target as HTMLInputElement;
    if (inputElement.checked) {
      this.selectedBotIds.push(bot.botId.toString());
      this.botManagement.addBot(bot);
    } else {
      this.selectedBotIds = this.selectedBotIds.filter(id => id !== bot.botId.toString());
    }
  }

  editBot(bot: Bot): void {
    console.log('Edit Bot:', bot);
    // Logik Bearbeiten Bot
  }
}
