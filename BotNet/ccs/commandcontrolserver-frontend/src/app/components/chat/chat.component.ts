import { Component } from '@angular/core';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent {
  messages: string[] = ['Willkommen im BotNet Terminal.'];
  command: string = '';

  executeCommand() {
    if (this.command.trim()) {
      this.messages.push(`> ${this.command}`);
      this.messages.push(`Bot-Antwort: Befehl "${this.command}" empfangen.`);
      this.command = '';
    }
  }
}
