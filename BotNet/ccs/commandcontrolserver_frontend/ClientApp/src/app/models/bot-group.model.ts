import { Bot } from './bot.model';

export interface BotGroup {
  botGroupId: number;
  name: string;
  createdAt: string;
  bots: Bot[];
}
