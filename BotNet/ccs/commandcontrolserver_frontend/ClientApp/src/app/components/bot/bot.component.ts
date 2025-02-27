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

  onBotClick() {
    this.toggleSelected.emit();
  }

  openEditDialog(event: MouseEvent) {
    event.stopPropagation();
    const dialogRef = this.dialog.open(BotEditDialogComponent, {
      data: { bot: this }
    });
  
    dialogRef.afterClosed().subscribe((updatedBot: Bot | undefined) => {
      if (updatedBot) {
        this.bot.name = updatedBot.name;
      }
    });
  }
  
  confirmDelete(event: MouseEvent): void {
    event.stopPropagation();

    if (confirm(`Are you sure you want to delete the bot ${this.bot.name}?`)) {
      this.botService.deleteBot(this.bot.botId).subscribe(() => {
        alert('Bot gelöscht');
        // Maybe refresh Bot-Liste
      });
    }
  }
}
