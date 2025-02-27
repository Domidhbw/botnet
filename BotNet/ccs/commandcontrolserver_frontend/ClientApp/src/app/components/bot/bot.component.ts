import { Component, Input, Inject } from '@angular/core';
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

  constructor(private botService: BotService, @Inject(MatDialog) private dialog: MatDialog) {}

  openEditDialog() {
    const dialogRef = this.dialog.open(BotEditDialogComponent, {
      data: { bot: this }
    });
  
    dialogRef.afterClosed().subscribe((updatedBot: Bot | undefined) => {
      if (updatedBot) {
        // Update the name of the bot after dialog is closed TODO
        this.bot.name = updatedBot.name;
      }
    });
  }
  

  confirmDelete(): void {
    if (confirm(`Are you sure you want to delete the bot ${this.bot.name}?`)) {
      this.botService.deleteBot(this.bot.botId).subscribe(() => {
        alert('Bot gelöscht');
        // Maybe Refresh Bot-Liste TODO
      });
    }
  }
}

