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
