import { Component } from '@angular/core';
import { Bot } from './components/bot-list/bot-list.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  showBotEditModal = false;
  showGroupsModal = false;
  showFileModal = false;

  onBotSelectionChanged(bots: Bot[]): void {
    console.log('Aktuelle Bot-Auswahl:', bots);
  }

  openBotEditModal(bot: Bot): void {
    console.log('Öffne Bot-Bearbeitungsmodal für:', bot);
    this.showBotEditModal = true;
  }

  openGroupsModal(): void {
    console.log('Öffne Gruppenmodal');
    this.showGroupsModal = true;
  }

  openFileModal(bot: Bot): void {
    console.log('Öffne Filemodal für:', bot);
    this.showFileModal = true;
  }
}
