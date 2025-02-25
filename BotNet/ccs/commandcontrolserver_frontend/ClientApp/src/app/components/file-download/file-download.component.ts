import { Component } from '@angular/core';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-file-download',
  templateUrl: './file-download.component.html',
  styleUrls: ['./file-download.component.css']
})
export class FileDownloadComponent {
  filePath = 'sample.txt';

  constructor(private apiService: ApiService) {}

  downloadFile() {
    this.apiService.downloadFile(this.filePath).subscribe({
      next: (blob) => {
        const a = document.createElement('a');
        const objectUrl = URL.createObjectURL(blob);
        a.href = objectUrl;
        a.download = this.filePath;
        a.click();
        URL.revokeObjectURL(objectUrl);
      },
      error: () => alert('File download failed!')
    });
  }
}
