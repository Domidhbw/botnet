import { Component } from '@angular/core';
import { CommandService } from '../../services/command.service';
import { BotResponse } from '../../models/bot-response.model';

@Component({
  selector: 'app-terminal',
  templateUrl: './terminal.component.html',
  styleUrls: ['./terminal.component.scss']
})
export class TerminalComponent {
  command: string = '';
  responses: BotResponse[] = [];
  filterBotId: number | null = null; // Optional: Filter nach Bot-ID

  constructor(private commandService: CommandService) {}

  sendCommand() {
    if (!this.command.trim()) {
      return;
    }
    // Sobald ein Befehl gesendet wird, wird der CommandService aufgerufen.
    // Dieser holt sich die aktuell ausgewählten Bots aus dem BotManagementService
    // und sendet den Befehl an jeden Bot.
    this.commandService.executeCommand(this.command).subscribe(resps => {
      this.responses = resps;
    });
    this.command = '';
  }

  filteredResponses(): BotResponse[] {
    if (this.filterBotId !== null) {
      return this.responses.filter(resp => resp.botId === this.filterBotId);
    }
    return this.responses;
  }
}
