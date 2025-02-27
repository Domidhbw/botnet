import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { forkJoin, Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { BotManagementService } from './bot-management.service';
import { Bot } from '../models/bot.model';
import { BotResponse } from '../models/bot-response.model';

@Injectable({
  providedIn: 'root'
})
export class CommandService {
  // API endpoints.
  private commandUrl = 'http://localhost:5002/api/Data/execute/command';
  private fileDownloadUrl = 'http://localhost:5002/api/Data/execute/file';

  constructor(
    private http: HttpClient,
    private botManagement: BotManagementService
  ) {}

  executeCommand(command: string): Observable<BotResponse[]> {
    const bots: Bot[] = this.botManagement.getSelectedBots();

    if (!bots || bots.length === 0) {
      return of([]);
    }

    const botIds = bots.map(bot => bot.botId);
    const payload = {
      botIds: botIds,
      command: command
    };

    return this.http.post<BotResponse[]>(this.commandUrl, payload)
    .pipe(
      catchError(err => {
        console.error('Error executing command', err);
        // Return an error response for each selected bot.
        const errorResponses = bots.map(bot => ({
          botResponseId: 0,
          botId: bot.botId,
          bot: {  // Populate bot object with necessary fields (based on your actual data)
            botId: bot.botId,
            dockerName: bot.dockerName,
            name: bot.name || '',
            lastAction: new Date().toISOString(),
            createdAt: new Date().toISOString(),
            updatedAt: new Date().toISOString(),
            botGroups: [],
            responses: []
          },
          responseType: 'command',
          success: false,
          timestamp: new Date().toISOString(),
          filePath: '',
          fileName: '',
          command: '',  // No command executed since there was an error
          responseContent: {  // New field for error responses
            command: '',  // No command executed due to error
            output: `Error: ${err.message}`
          }
        } as BotResponse));
        
        return of(errorResponses);
      })
    );
  }

  downloadFile(filePath: string): void {
    const bots: Bot[] = this.botManagement.getSelectedBots();

    if (!bots || bots.length === 0) {
      console.warn('No bots selected.');
      return;
    }

    const botIds = bots.map(bot => bot.botId);
    const payload = {
      botIds: botIds,
      filePath: filePath
    };

    this.http.post(this.fileDownloadUrl, payload, { responseType: 'blob' })
      .pipe(
        catchError(err => {
          console.error('Error downloading file', err);
          return of(null);
        })
      )
      .subscribe(blob => {
        if (blob) {
          // Create a temporary link element and trigger the download.
          const a = document.createElement('a');
          const objectUrl = URL.createObjectURL(blob);
          a.href = objectUrl;
          // Extract a filename from the provided filePath or default to 'download'
          a.download = filePath.split('/').pop() || 'download';
          a.click();
          URL.revokeObjectURL(objectUrl);
        }
      });
  }
}
