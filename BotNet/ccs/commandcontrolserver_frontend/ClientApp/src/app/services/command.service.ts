import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { forkJoin, Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { BotResponse } from '../models/bot-response.model';
import { BotManagementService } from './bot-management.service';
import { Bot } from '../models/bot.model';

@Injectable({
  providedIn: 'root'
})
export class CommandService {
  private commandUrl = 'http://localhost:5001/api/command/run';

  constructor(private http: HttpClient, private botManagement: BotManagementService) {}

  executeCommand(cmd: string): Observable<BotResponse[]> {
    const bots: Bot[] = this.botManagement.getSelectedBots();

    if (!bots || bots.length === 0) {
      return of([]);
    }

    // Für jeden ausgewählten Bot wird ein Request abgesetzt.
    const requests = bots.map(bot => {
      return this.http.get<BotResponse>(
        `${this.commandUrl}?cmd=${encodeURIComponent(cmd)}&botId=${bot.botId}`
      ).pipe(
        catchError(err => {
          return of({
            botResponseId: 0,
            botId: bot.botId,
            responseType: 'command',
            success: false,
            timestamp: new Date().toISOString(),
            filePath: '',
            fileName: ''
          } as BotResponse);
        })
      );
    });

    return forkJoin(requests);
  }
}
