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
> URL: `http://localhost:5002`
