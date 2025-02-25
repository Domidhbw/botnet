import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private baseUrl = 'http://localhost:5001/api';

  constructor(private http: HttpClient) { }

  // Call the command API and return text response
  runCommand(cmd: string): Observable<string> {
    return this.http.get(`${this.baseUrl}/command/run?cmd=${cmd}`, { responseType: 'text' });
  }

  // Download the file
  downloadFile(filepath: string): Observable<Blob> {
    return this.http.get(`${this.baseUrl}/file/download?filepath=${filepath}`, { responseType: 'blob' });
  }
}
