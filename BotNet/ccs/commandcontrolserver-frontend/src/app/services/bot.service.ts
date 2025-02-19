import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class BotService {
  getBots() {
    return [
      { id: 'bot1', name: 'Bot 1', status: 'Aktiv' },
      { id: 'bot2', name: 'Bot 2', status: 'Inaktiv' },
      { id: 'bot3', name: 'Bot 3', status: 'Aktiv' },
    ];
  }
}
