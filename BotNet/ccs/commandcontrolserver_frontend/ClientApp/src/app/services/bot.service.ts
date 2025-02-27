import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Bot } from '../models/bot.model';

@Injectable({
  providedIn: 'root'
})
export class BotService {
  private apiUrl = 'http://localhost:5002/api/Bot/';

  constructor(private http: HttpClient) {}

  getBots(): Observable<Bot[]> {
    return this.http.get<Bot[]>(`${this.apiUrl}bots`);
  }

  getBot(id: number): Observable<Bot> {
    return this.http.get<Bot>(`${this.apiUrl}bot/${id}`);
  }

  createBot(port: number): Observable<Bot> {
    return this.http.post<Bot>(`${this.apiUrl}bot`, { port });
  }

  updateBot(botId: number, newName: string): Observable<void> {
    const headers = { 'Content-Type': 'text/plain' }; 
    return this.http.put<void>(`${this.apiUrl}editName/${botId}`, newName, { headers });
  }
  
  

  deleteBot(botId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}bot/${botId}`);
  }
}
