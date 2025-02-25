export interface BotResponse {
    botResponseId: number;
    botId: number;
    responseType: 'command' | 'file';
    success: boolean;
    timestamp: string;
    filePath?: string;
    fileName?: string;
  }
  