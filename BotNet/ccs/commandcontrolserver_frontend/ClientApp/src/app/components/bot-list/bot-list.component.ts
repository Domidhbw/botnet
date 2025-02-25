import { Component, OnInit, Output, EventEmitter } from '@angular/core';

export interface Bot {
  id: string;
  name: string;
  selected?: boolean;
}

@Component({
  selector: 'app-bot-list',
  imports: [],
  templateUrl: './bot-list.component.html',
  styleUrls: ['./bot-list.component.scss']
})

export class BotListComponent implements OnInit {
  bots: Bot[] = [
    { id: 'bot1', name: 'Bot 1', selected: true },
    { id: 'bot2', name: 'Bot 2', selected: false },
    { id: 'bot3', name: 'Bot 3', selected: false },
    { id: 'bot4', name: 'Bot 4', selected: false },
    { id: 'bot5', name: 'Bot 5', selected: false }
  ];

  // Events, um Aktionen an die übergeordnete Komponente zu senden:
  @Output() botSelectionChanged = new EventEmitter<Bot[]>();
  @Output() editBot = new EventEmitter<Bot>();
  @Output() fileBot = new EventEmitter<Bot>();
  @Output() openGroups = new EventEmitter<void>();

  constructor() { }

  ngOnInit(): void { }

  onBotSelectionChange(bot: Bot): void {
    // Toggle-Auswahl
    bot.selected = !bot.selected;
    this.botSelectionChanged.emit(this.bots);
  }

  openBotEditModal(bot: Bot): void {
    this.editBot.emit(bot);
  }

  openFileModal(bot: Bot): void {
    this.fileBot.emit(bot);
  }

  openGroupsModal(): void {
    this.openGroups.emit();
  }
}
