import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { forkJoin, Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { BotResponse } from '../models/bot-response.model';
import { BotManagementService } from './bot-management.service';
import { Bot } from '../models/bot.model';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class DownloadService {
  private downloadUrl = 'http://localhost:5002/api/Data/execute/command';

  constructor(private http: HttpClient, private botManagement: BotManagementService) {}

  executedownload(cmd: string): Observable<BotResponse[]> {
    const bots: Bot[] = this.botManagement.getSelectedBots();

    if (!bots || bots.length === 0) {
      return of([]);
    }

    // Für jeden ausgewählten Bot wird ein Request abgesetzt.
    const requests = bots.map(bot => {
      // Hier wird der HTTP-Request so konfiguriert, dass er reinen Text zurückgibt.
      return this.http.get(
        `${this.downloadUrl}?cmd=${encodeURIComponent(cmd)}&botId=${bot.botId}`,
        { responseType: 'text' }
      ).pipe(
        // Den Text in ein BotResponse-Objekt umwandeln.
      map((text: string) => {
          const response: BotResponse = {
            botResponseId: 0,
            botId: bot.botId,
            responseType: 'command',
            success: true,
            timestamp: new Date().toISOString(),
            // Wir speichern den reinen Text hier in der neuen Property "output"
            output: text,
            filePath: '',
            fileName: ''
          };
          return response;
        }),
        catchError(err => {
          // Bei einem Fehler wird ein Fehlerobjekt zurückgegeben.
          return of({
            botResponseId: 0,
            botId: bot.botId,
            responseType: 'command',
            success: false,
            timestamp: new Date().toISOString(),
            output: `Error: ${err.message}`,
            filePath: '',
            fileName: ''
          } as BotResponse);
        })
      );
    });

    // Alle Requests werden parallel ausgeführt.
    return forkJoin(requests);
  }
}
