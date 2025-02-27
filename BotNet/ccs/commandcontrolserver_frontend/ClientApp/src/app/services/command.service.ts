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
  private commandUrl = 'http://localhost:5002/api/Data/execute/command';
  private downloadUrl = 'http://localhost:5002/api/Data/download';

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
        const errorResponses = bots.map(bot => ({
          botResponseId: 0,
          botId: bot.botId,
          bot: {  
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
          command: '',  
          responseContent: {  
            command: '', 
            output: `Error: ${err.message}`
          }
        } as BotResponse));
        
        return of(errorResponses);
      })
    );
  }

  downloadFile(filePath: string): void {
    const bots: Bot[] = this.botManagement.getSelectedBots();
    const fileName = filePath.split('/').pop();
  
    if (!bots || bots.length === 0) {
      console.warn('No bots selected.');
      return;
    }
  
    const botIds = bots.map(bot => bot.botId);
    const url = `${this.downloadUrl}?botIds=${botIds.join('&botIds=')}&filePath=${encodeURIComponent(filePath)}`;

    try {
      fetch(url, {
        method: 'GET', 
      })
      .then(response => {
        if (!response.ok) {
          throw new Error(`Error: ${response.statusText}`);
        }
        return response.blob();
      })
      .then(blob => {
        const objectUrl = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = objectUrl;
        a.download = fileName+'.zip';
        document.body.appendChild(a);
        a.click();
        a.remove();
        window.URL.revokeObjectURL(objectUrl);
      })
      .catch(error => {
        console.error('Error downloading the file:', error);
      });
    } catch (error) {
      console.error('Fetch error:', error);
    }
  }
}
