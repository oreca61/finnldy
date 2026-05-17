
## Name: Finnldy
### Entwickler: Muhammet Altuntas / Jakob Seeberger

## Projektplan
### Projektbeschreibung:
```
Die Anwendung ist eine Gruppen-Filmempfehlungs-App wie „Tinder für Filme“. Nutzer können gemeinsam mit Freunden oder Familie einer Lobby beitreten. Anschließend werden allen Teilnehmern Filme aus einer Datenbank vorgeschlagen. Jeder Nutzer kann einen Film liken, disliken oder als „schon gesehen“ markieren. Sobald alle Teilnehmer denselben Film geliked haben, wird dieser als gemeinsames Ergebnis angezeigt. Filme, die von einem Nutzer als „schon gesehen“ markiert wurden, werden aus der Auswahl ausgeschlossen. Falls nach 50 Swipes kein perfekter Treffer gefunden wurde, wird der Film mit der besten Bewertung angezeigt. Zusätzlich gibt es „Honorable Mentions“, bei denen die Plätze 2 bis 5 angezeigt werden. Nutzer können Filme außerdem zu einer „Später ansehen“-Liste hinzufügen und im Hauptmenü ihre gesehenen sowie gespeicherten Filme ansehen.
```

### Zuständigkeiten

#### Jakob:
##### Bll:
- Lobby
- Swipe-Klassen
- resultklassen
##### GUI:
-  Startseite(Fenster) & kleines Lobby fenster
##### DAL:
- Database-Klasse(alles was mit laden/Lesen zutun hat)
- Ireposotory
- Networkservive client-teil


#### Muhammet:
##### Bll:
- Users-Klasse
- Movie + Rep
##### GUI:
- Filmkarte anzeigen(Cover,
Titel,
Beschreibung,
Genres)
- Nebenfenster(Schongesehen, resultview usw.)
##### DAL:
- Networkservive Host-teil
- Databse-Klasse(alles was mit speichern zu tun hat)


#### Gemeinsam:
- Lobby-Klasse
- LobbyController-Klasse
- Temporäre Sachen wie swipe view oder Resultviewe



### Milestones
#### bis 24.05:
- Movie +Repository
- users
- Startseite
- LobbyController
- Startfenster
- Filmkarten-GUI Grundlayout

anfangen:
- Database Lesen/Schreiben
- swipeview
- Resultview

#### bis 31.05
- Movies laden
- API-Verbindung
- JSON lesen/deserialisieren
- resultKlassen
- swipeklassen

anfangen:
- Lobby
- HandyLobby
- Networking

fertig machen von:
- Database Lesen/Schreiben

#### bis 07.06
- HandyGUI

fertig machen von:
- Lobby
- HandyLobby



## UMLdiagramm
![alt text](image.png)

## UI
![alt text](image-2.png)
