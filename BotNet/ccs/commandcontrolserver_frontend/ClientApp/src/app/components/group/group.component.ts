import { Component, Input } from '@angular/core';
import { BotGroup } from '../../models/bot-group.model';
import { BotGroupService } from '../../services/bot-group.service';
import { Bot } from '../../models/bot.model';

@Component({
  selector: 'app-group',
  templateUrl: './group.component.html',
  styleUrls: ['./group.component.scss']
})
export class GroupComponent {
  @Input() group!: BotGroup;
  editing: boolean = false;
  newName: string = '';
  // Lokale Kopie der Bots der Gruppe für Änderungen
  selectedBots: Bot[] = [...this.group.bots];

  constructor(private groupService: BotGroupService) {}

  enableEdit() {
    this.editing = true;
    this.newName = this.group.name;
    this.selectedBots = [...this.group.bots];
  }

  saveGroup() {
    // API-Aufruf zur Aktualisierung des Gruppennamens (und idealerweise auch der Bots)
    this.groupService.updateGroup(this.group.botGroupId, this.newName).subscribe(updatedGroup => {
      this.group.name = this.newName;
      this.editing = false;
    });
  }

  confirmDelete() {
    if (confirm(`Bist du sicher, dass du die Gruppe ${this.group.name} löschen möchtest?`)) {
      this.groupService.deleteGroup(this.group.botGroupId).subscribe(() => {
        alert('Gruppe gelöscht');
      });
    }
  }

  // Beispiel-Methoden zum Hinzufügen/Entfernen von Bots
  addBot(bot: Bot) {
    if (!this.selectedBots.find(b => b.botId === bot.botId)) {
      this.selectedBots.push(bot);
    }
  }

  removeBot(bot: Bot) {
    this.selectedBots = this.selectedBots.filter(b => b.botId !== bot.botId);
  }
}
