import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Bot } from '../models/bot.model';
import { BotService } from '../services/bot.service';

@Component({
  selector: 'app-bot-edit-dialog',
  templateUrl: './bot-edit-dialog.component.html',
  styleUrls: ['./bot-edit-dialog.component.scss']
})
export class BotEditDialogComponent {
  newName: string;
  botId: number;

  constructor(
    public dialogRef: MatDialogRef<BotEditDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { bot: Bot },
    private botService: BotService
  ) {
    this.newName = data.bot.name;
    this.botId = data.bot.botId;
  }

  saveName(): void {
    this.botService.updateBot(this.botId, this.newName).subscribe({
      next: () => {
        this.dialogRef.close({ name: this.newName, botId: this.botId });
      },
      error: (err) => {
        console.error('Error updating bot name:', err);
        alert('There was an error updating the bot name.');
      }
    });
  }
  

  deleteBot(): void {
    if (confirm('Are you sure you want to delete this bot?')) {
      this.botService.deleteBot(this.botId).subscribe(() => {
        alert('Bot deleted');
        this.dialogRef.close(); // Optionally, you might want to pass a flag that deletion occurred.
      });
    }
  }

  cancel(): void {
    this.dialogRef.close();
  }
}
