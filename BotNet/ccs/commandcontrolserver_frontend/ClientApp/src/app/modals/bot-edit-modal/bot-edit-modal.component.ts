import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-bot-edit-modal',
  templateUrl: './bot-edit-modal.component.html',
  styleUrls: ['./bot-edit-modal.component.scss']
})
export class BotEditModalComponent {
  @Output() closeModal = new EventEmitter<void>();

  close(): void {
    this.closeModal.emit();
  }
}
