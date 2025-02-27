import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { BotListComponent } from './components/bot-list/bot-list.component';
import { GroupListComponent } from './components/group-list/group-list.component';
import { TerminalComponent } from './components/terminal/terminal.component';
import { BotComponent } from './components/bot/bot.component';
import { GroupComponent } from './components/group/group.component';

@NgModule({
  declarations: [
    AppComponent,
    BotListComponent,
    GroupListComponent,
    TerminalComponent,
    BotComponent,
    GroupComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
