export interface BotResponse {
  botResponseId: number;
  botId: number;
  responseType: 'command' | 'file';
  success: boolean;
  timestamp: string;
  // Neue Property zur Speicherung des Befehlsausgabe-Textes:
  output?: string;
  filePath?: string;
  fileName?: string;
}
