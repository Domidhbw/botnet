import { Component, Input, Output, EventEmitter, Inject } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Bot } from '../../models/bot.model';
import { BotService } from '../../services/bot.service';
import { BotEditDialogComponent } from '../../popups/bot-edit-dialog.component';

@Component({
  selector: 'app-bot',
  templateUrl: './bot.component.html',
  styleUrls: ['./bot.component.scss']
})
export class BotComponent {
  @Input() bot!: Bot;
  @Input() isSelected: boolean = false;
  @Output() toggleSelected = new EventEmitter<void>();

  constructor(
    private botService: BotService,
    @Inject(MatDialog) private dialog: MatDialog
  ) {}

  /** Wenn man auf den Bot klickt, soll das Selection-Toggle ausgelöst werden. */
  onBotClick() {
    this.toggleSelected.emit();
  }

  openEditDialog(event: MouseEvent) {
    // Verhindert, dass das Klicken auf den Stift auch den gesamten Bot auswählt.
    event.stopPropagation();
    const dialogRef = this.dialog.open(BotEditDialogComponent, {
      data: { bot: this.bot }
    });
  
    dialogRef.afterClosed().subscribe((updatedBot: Bot | undefined) => {
      if (updatedBot) {
        this.bot.name = updatedBot.name;
      }
    });
  }
  
  confirmDelete(event: MouseEvent): void {
    // Ebenfalls Selektion verhindern
    event.stopPropagation();

    if (confirm(`Are you sure you want to delete the bot ${this.bot.name}?`)) {
      this.botService.deleteBot(this.bot.botId).subscribe(() => {
        alert('Bot gelöscht');
        // Maybe refresh Bot-Liste
      });
    }
  }
}
