import { Component } from '@angular/core';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent {
  bots = [
    { id: 'bot1', name: 'Bot 1', checked: true },
    { id: 'bot2', name: 'Bot 2', checked: false },
    { id: 'bot3', name: 'Bot 3', checked: false },
  ];

  toggleBot(bot: any) {
    console.log(`${bot.name} ${bot.checked ? 'ausgewählt' : 'abgewählt'}`);
  }

  editBot(bot: any) {
    alert(`Bearbeite ${bot.name}`);
  }

  showFiles(bot: any) {
    alert(`Zeige Dateien von ${bot.name}`);
  }

  openGroups() {
    alert('Gruppen-Verwaltung öffnen');
  }
}
