# MA-Control TODO
## IP-Adresse des PCs:
192.168.2.69
24
192.168.2.1

**Matrix-Control** shows the logic of the program.
## TODO
- [x] Das Spiel soll mit der Leer-Taste starten.
- [x] Nach 'Game Over' soll das Spiel mit der Leer-Taste neu gestartet werden.
- [x] Bei 'Game Over' soll der Dino rot gezeichnet werden. Beim Neustart des Spiels soll er wieder grün sein.
- [x] Sekundenzähler = score hinzufügen
- [x] High score hinzufügen
- [ ] Selber eine Verbesserung im Programm machen
	- [x] Hitboxen sind 15 Pixel hinter dem Angezeigten Obstacle (rechte seite dino bis linke seite obstacle)
	- [X] Sprunghöhe nicht linear sondern mit gravity und acceleration
	- [x] neu generierte Map weiter hinten starten
	- [x] wenn man während collision springt ist game over false, sobald man oberhalb des obstacles ist 
		  und spiel kann sogar weiter gehen, wenn man trotzdem darüber kommt
	- [x] highscore in json speichern
	- [x] hindernisse vorne entfernen und hinten neue einfügen statt konstant 50, die ausgehen
	- [x] pfade relativ machen
	- [x] title screen hinzufügen
	- [x] Geschwindigkeit der Hindernisse über Zeit schneller
	- [x] Schwierigkeit wird angezeigt
	- [x] Schwierigkeit per tastendruck im game over screen verstellbar
	- [ ] highscores werden je nach schwierigkeit separat gespeichert
		ERROR highscore für difficulty wird erst beim nächsten gameover aktualisiert
	- [ ] _timestamp (DisplayContent) updaten sodass es deltaTime entspricht 
		<-> testen ob man springen kann nach Position nicht Zeit
	- [ ] fliegende hindernisse
	- [ ] Images als Textur verwenden statt Point/Edge Objekte zu erstellen