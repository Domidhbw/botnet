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
export interface Bot {
  botId: number;
  dockerName: string;
  name: string;
  lastAction: string;
  createdAt: string;
  updatedAt: string;
  botGroups: any[]; 
  responses: any[]; 
}
