import { Component } from '@angular/core';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-command-output',
  templateUrl: './command-output.component.html',
  styleUrls: ['./command-output.component.css']
})
export class CommandOutputComponent {
  command = 'ls'; // Default command
  output: string = '';

  constructor(private apiService: ApiService) {}

  runCommand() {
    this.apiService.runCommand(this.command).subscribe({
      next: (response) => this.output = response,
      error: (error) => this.output = 'Error running command'
    });
  }
}
