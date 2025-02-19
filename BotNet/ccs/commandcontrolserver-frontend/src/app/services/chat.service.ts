import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  sendCommand(command: string): string {
    return `Befehl "${command}" erfolgreich ausgeführt!`;
  }
}
