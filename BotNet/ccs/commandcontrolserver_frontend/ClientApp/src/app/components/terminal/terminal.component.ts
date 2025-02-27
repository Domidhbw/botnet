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
  
  showDropdown = false;
  selectedBotIds: number[] = []; 

  constructor(private commandService: CommandService) {}

  sendCommand() {
    if (!this.command.trim()) {
      return;
    }
    this.commandService.executeCommand(this.command).subscribe(resps => {
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

  uniqueBotIds(): number[] {
    const setOfIds = new Set<number>();
    this.responses.forEach(resp => setOfIds.add(resp.botId));
    return Array.from(setOfIds).sort();
  }

  toggleDropdown() {
    this.showDropdown = !this.showDropdown;
  }

  isBotIdSelected(botId: number): boolean {
    return this.selectedBotIds.includes(botId);
  }

  toggleBotId(botId: number, event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.checked) {
      if (!this.selectedBotIds.includes(botId)) {
        this.selectedBotIds.push(botId);
      }
    } else {
      const idx = this.selectedBotIds.indexOf(botId);
      if (idx !== -1) {
        this.selectedBotIds.splice(idx, 1);
      }
    }
  }

  filteredResponses(): BotResponse[] {
    if (this.selectedBotIds.length === 0) {
      return this.responses;
    } else {
      return this.responses.filter(resp => this.selectedBotIds.includes(resp.botId));
    }
  }

  get disableDownload(): boolean {
    return this.selectedBotIds.length > 1;
  }
}
