import { Component, Input } from '@angular/core';
import { Bot } from '../../models/bot.model';
import { BotService } from '../../services/bot.service';

@Component({
  selector: 'app-bot',
  templateUrl: './bot.component.html',
  styleUrls: ['./bot.component.scss']
})
export class BotComponent {
  @Input() bot!: Bot;
  editing: boolean = false;
  newName: string = '';

  constructor(private botService: BotService) {}

  enableEdit() {
    this.editing = true;
    this.newName = this.bot.name;
  }

  saveName() {
    this.botService.updateBot(this.bot.botId).subscribe(updatedBot => {
      this.bot.name = this.newName;
      this.editing = false;
    });
  }

  confirmDelete() {
    if (confirm(`Bist du sicher, dass du den Bot ${this.bot.name} löschen möchtest?`)) {
      this.botService.deleteBot(this.bot.botId).subscribe(() => {
        alert('Bot gelöscht');
        // Maybe Refresh Bot-Liste
      });
    }
  }
}
