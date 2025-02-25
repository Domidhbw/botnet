import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BotGroup } from '../models/bot-group.model';

@Injectable({
  providedIn: 'root'
})
export class BotGroupService {
  private apiUrl = 'http://localhost:5002/api/BotGroup/';

  constructor(private http: HttpClient) {}

  getGroups(): Observable<BotGroup[]> {
    return this.http.get<BotGroup[]>(`${this.apiUrl}botGroup`);
  }

  getGroup(id: number): Observable<BotGroup> {
    return this.http.get<BotGroup>(`${this.apiUrl}botGroup/${id}`);
  }

  createGroup(name: string): Observable<BotGroup> {
    return this.http.post<BotGroup>(`${this.apiUrl}botGroup`, { name });
  }

  updateGroup(botGroupId: number, name: string): Observable<BotGroup> {
    return this.http.put<BotGroup>(`${this.apiUrl}botGroup/${botGroupId}`, { botGroupId, name });
  }

  deleteGroup(botGroupId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}botGroup/${botGroupId}`);
  }
}
