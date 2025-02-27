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
  filePath: string = '';
  responses: BotResponse[] = [];
  filterBotId: number | null = null; // Filter by Bot ID

  constructor(
    private commandService: CommandService,
  ) {}


  sendCommand() {
    if (!this.command.trim()) {
      return;
    }
    this.commandService.executeCommand(this.command).subscribe(resps => {
      // Append or replace responses as desired.
      this.responses = resps;
    });
    this.command = '';
  }


  downloadFile() {
    if (!this.filePath.trim()) {
      return;
    }
    this.commandService.downloadFile(this.filePath);
    this.filePath = '';
  }

  filteredResponses(): BotResponse[] {
    if (this.filterBotId !== null) {
      return this.responses.filter(resp => resp.botId === this.filterBotId);
    }
    return this.responses;
  }
}
