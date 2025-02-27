import { Bot } from './bot.model';

export interface BotResponse {
  botResponseId: number;
  botId: number;
  bot: Bot;
  responseType: 'command' | 'file';
  success: boolean;
  timestamp: string;
  filePath?: string;
  fileName?: string;
  command?: string;
  responseContent?: ResponseContent; // Add responseContent as a structured object
}

export interface ResponseContent {
  command: string;
  output: string;
}
