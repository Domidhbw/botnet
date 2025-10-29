import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';

import { AppComponent } from './app.component';
import { BotListComponent } from './components/bot-list/bot-list.component';
import { GroupListComponent } from './components/group-list/group-list.component';
import { TerminalComponent } from './components/terminal/terminal.component';
import { BotComponent } from './components/bot/bot.component';
import { GroupComponent } from './components/group/group.component';
import { BotEditDialogComponent } from './popups/bot-edit-dialog.component';

@NgModule({ declarations: [
        AppComponent,
        BotListComponent,
        GroupListComponent,
        TerminalComponent,
        BotComponent,
        GroupComponent,
        BotEditDialogComponent,
    ],
    bootstrap: [AppComponent], imports: [BrowserModule,
        MatDialogModule,
        MatButtonModule,
        FormsModule], providers: [provideHttpClient(withInterceptorsFromDi())] })
export class AppModule { }
