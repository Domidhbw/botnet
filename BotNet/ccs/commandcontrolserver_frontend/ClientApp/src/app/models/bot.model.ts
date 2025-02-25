export interface Bot {
    botId: number;
    port: number;
    name: string;
    status: string;
    lastSeen: string;
    createdAt: string;
    updatedAt: string;
    responses: any[];
    botGroups: any[];
  }
  