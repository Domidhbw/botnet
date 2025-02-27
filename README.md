# BotNet

## Beschreibung
BotNet stellt eine Simulation eines Bot-Netzwerks dar. Es besteht aus einem ccs-Server, einer Benutzer-Oberfläche und mehreren Bot-APIs.
Wird eine Bot-API gestartet, so verbindet sie sich mit dem ccs-Server und ist bereit, Befehle entgegenzunehmen. Die Benutzer-Oberfläche ermöglicht es, Befehle an die Bot-APIs zu senden und die Ergebnisse zu betrachten.
So können auf einfache Weise Angriffe in Form von Konsolen-Befehlen und Datei-Diebstahl simuliert werden.


## Programmstart
### Voraussetzungen
Docker und Docker-Compose müssen installiert sein. Per Default werden außerdem die Ports 5002 und 5003 benötigt.

### Starten
1. Navigieren Sie in das BotNet/BotNet-Verzeichnis
2. (Beim ersten Start) Führen Sie den Befehl `docker-compose build` aus. Dies dürfte einige Zeit in Anspruch nehmen.
3. Führen Sie den Befehl `docker-compose up --scale bot-api=10 -d` aus. `bot-api=10` gibt an, dass 10 Instanzen der Bot-API gestartet werden sollen. Dieser Wert kann beliebig angepasst werden.
4. Die Benuzter-Oberfläche ist nun unter `http://localhost:5003` erreichbar.


## API-Endpunkte mit Erklärung
### Bot-API
> Diese Endpunkte sind nur für das ccs-Server-System relevant.
- `GET` `online/onlineStatus`: Gibt eine Antwort zurück, falls der Bot erreichbar ist.
- `GET` `file/download`: Gibt eine Datei zurück, die auf dem Bot gespeichert ist.
- `GET` `command/run`: Führt einen Befehl auf dem Bot aus und gibt das Ergebnis zurück.

### CCS-Server
> URL: `http://localhost:5002/api/`

**Bot Controller**
> URL: `http://localhost:5002/api/bot/`

- `GET` `bots`: Gibt alle Bots zurück.
- `GET` `bots/{id}`: Gibt einen Bot zurück.	
- `POST` `bots`: Erstellt einen neuen Bot. Es sind keine Parameter notwendig, da der Containername automatisch ausgelesen wird.
- `PUT` `editName/{id}`: Ändert den Namen eines Bots. Body: { "name": "neuerName" }
- `PUT` `editBotGroups/{id}`: Ändert die Gruppen eines Bots. 
- `DELETE` `bots/{id}`: Löscht einen Bot.
  
**BotGroup Controller**
> URL: `http://localhost:5002/api/botgroup/`

> Mangels Zeit wurde die Implementierung der Bot-Gruppen im Frontend nicht fertiggestellt. Die API-Endpunkte sind jedoch vorhanden und voll funktionsfähig.

- `GET` `botgroups`: Gibt alle Bot-Gruppen zurück.
- `GET` `botgroups/{id}`: Gibt eine Bot-Gruppe zurück.
- `POST` `botgroups`: Erstellt eine neue Bot-Gruppe. Body: { "name": "groupName" }
- `PUT` `editName/{id}`: Ändert den Namen einer Bot-Gruppe. Body: { "name": "neuerName" }
- `DELETE` `botgroups/{id}`: Löscht eine Bot-Gruppe.

**BotResponseController**
> URL: `http://localhost:5002/api/botresponse/`

- `GET` `botresponses`: Gibt alle Bot-Antworten zurück.
- `GET` `botresponses/{id}`: Gibt eine Bot-Antwort zurück.
- `POST` `botresponse`: Erstellt eine neue Bot-Antwort. Body: { "botId": 1, "responseType": "command" }

**DataController**
> URL: `http://localhost:5002/api/data/`

- `POST` `execute/file`: Lädt eine Datei von den angegebenen Bots ins ccs herunter. Body: { "botIds": [1, 2], "filePath": "web.config" }
- `POST` `execute/command`: Führt einen Befehl auf den angegebenen Bots aus und gibt das Ergebnis zurück. Body: { "botIds": [1, 2], "command": "dir" }
- `GET` `download`: Lädt Dateien von den angegebenen Bots herunter. Parameter: botIds, filePath (z.B. `http://localhost:5002/api/data/download?botIds=1&filePath=web.config`)


## Verwendete Technologien
Die Backends basieren auf ASP.NET Core, während das Frontend in Angular geschrieben ist. Die Kommunikation zwischen den Komponenten erfolgt über REST-APIs. Die Datenbank ist eine SQLite-Datenbank. Die gesamte Anwendung ist in Docker-Containern verpackt, wobei Das Frontend und jeder Bot in einem eigenen Container laufen. Die Datenbank und der ccs-Server laufen in einem gemeinsamen Container.