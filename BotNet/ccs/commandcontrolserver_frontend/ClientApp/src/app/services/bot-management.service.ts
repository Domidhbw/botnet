import { Injectable } from '@angular/core';
import { Bot } from '../models/bot.model';

@Injectable({
  providedIn: 'root'
})
export class BotManagementService {
  private selectedBots: Bot[] = [];

  getSelectedBots(): Bot[] {
    return this.selectedBots;
  }

  addBot(bot: Bot): void {
    if (!this.selectedBots.find(b => b.botId === bot.botId)) {
      this.selectedBots.push(bot);
    }
  }

  removeBot(bot: Bot): void {
    this.selectedBots = this.selectedBots.filter(b => b.botId !== bot.botId);
  }

  // Fügt eine Gruppe von Bots hinzu (beachtet Duplikate)
  addBots(bots: Bot[]): void {
    bots.forEach(bot => this.addBot(bot));
  }
  //darauf achten ob die bots aufgrund einer anderen gruppe hier sein sollten TODO
  // Entfernt alle Bots der übergebenen Gruppe
  removeBots(bots: Bot[]): void {
    bots.forEach(bot => this.removeBot(bot));
  }
}
