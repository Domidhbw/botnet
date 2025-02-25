import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { trigger, transition, style, animate } from '@angular/animations';

interface Message {
  text: string;
  bot?: string;
}

interface FilterOption {
  name: string;
  checked: boolean;
}

@Component({
  selector: 'app-terminal',
  templateUrl: './terminal.component.html',
  styleUrls: ['./terminal.component.scss'],
  animations: [
    trigger('dropdownAnimation', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateY(-10px)' }),
        animate('200ms ease-out', style({ opacity: 1, transform: 'translateY(0)' }))
      ]),
      transition(':leave', [
        animate('200ms ease-in', style({ opacity: 0, transform: 'translateY(-10px)' }))
      ])
    ])
  ]
})
export class TerminalComponent implements OnInit {
  @ViewChild('terminalOutput') terminalOutput!: ElementRef;

  messages: Message[] = [
    { text: 'Willkommen im BotNet Terminal.' }
  ];
  command: string = '';
  dropdownVisible: boolean = false;
  filterBtnLabel: string = 'Alle Bots';
  filterOptions: FilterOption[] = [
    { name: 'Alle', checked: true },
    { name: 'Bot 1', checked: true },
    { name: 'Bot 2', checked: true },
    { name: 'Bot 3', checked: true },
    { name: 'Bot 4', checked: true },
    { name: 'Bot 5', checked: true }
  ];

  constructor() { }

  ngOnInit(): void { }

  toggleDropdown(event: MouseEvent): void {
    event.stopPropagation();
    this.dropdownVisible = !this.dropdownVisible;
  }

  updateFilter(): void {
    const selected = this.filterOptions.filter(opt => opt.name !== 'Alle' && opt.checked).map(opt => opt.name);
    if (selected.length === 0) {
      this.filterBtnLabel = 'Keine Bots';
    } else if (selected.length === 5) {
      this.filterBtnLabel = 'Alle Bots';
    } else {
      this.filterBtnLabel = 'Angepasst';
    }
  }

  shouldDisplay(msg: Message): boolean {
    if (!msg.bot) return true;
    const filter = this.filterOptions.find(opt => opt.name === msg.bot);
    return filter ? filter.checked : false;
  }

  appendMessage(message: string): void {
    this.messages.push({ text: message });
    this.scrollToBottom();
  }

  appendBotResponse(botName: string, message: string): void {
    this.messages.push({ text: `<${botName}> ${message}`, bot: botName });
    this.scrollToBottom();
  }

  executeCommand(): void {
    const cmd = this.command.trim();
    if (!cmd) return;
    this.appendMessage(`> ${cmd}`);
    this.filterOptions.forEach(option => {
      if (option.name !== 'Alle' && option.checked) {
        if (cmd.toLowerCase() === 'status') {
          this.appendBotResponse(option.name, 'Aktiv');
        } else if (cmd.toLowerCase().startsWith('run')) {
          this.appendBotResponse(option.name, `Befehl "${cmd}" wird ausgeführt...`);
        } else if (cmd.toLowerCase() === 'help') {
          this.appendBotResponse(option.name, `"status" - Zeigt Status, "run" - Führt einen Command aus, "help" - Zeigt diese Hilfsliste`);
        } else {
          this.appendBotResponse(option.name, `Unbekannter Befehl: ${cmd}. Nutze "help" für eine Liste an Commands`);
        }
      }
    });
    this.command = '';
  }

  scrollToBottom(): void {
    setTimeout(() => {
      this.terminalOutput.nativeElement.scrollTop = this.terminalOutput.nativeElement.scrollHeight;
    }, 0);
  }
}
