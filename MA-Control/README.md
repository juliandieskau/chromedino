# MA-Control

**Matrix-Control** shows the logic of the program.
## TODO
- [x] Das Spiel soll mit der Leer-Taste starten.
- [x] Nach 'Game Over' soll das Spiel mit der Leer-Taste neu gestartet werden.
- [x] Bei 'Game Over' soll der Dino rot gezeichnet werden. Beim Neustart des Spiels soll er wieder grün sein.
- [x] Sekundenzähler = score hinzufügen
- [x] High score hinzufügen
- [ ] Selber eine Verbesserung im Programm machen
	- [x] Hitboxen sind 15 Pixel hinter dem Angezeigten Obstacle (rechte seite dino bis linke seite obstacle)
	- [ ] Geschwindigkeit der Hindernisse über Zeit schneller?
	- [X] Sprunghöhe nicht linear sondern mit gravity und acceleration
	- [x] neu generierte Map weiter hinten starten
	- [ ] wenn man während collision springt ist game over false, sobald man oberhalb des obstacles ist 
		  und spiel kann sogar weiter gehen, wenn man trotzdem darüber kommt
	- [ ] _timestamp (DisplayContent) updaten sodass es deltaTime entspricht 
		<-> testen ob man springen kann nach Position nicht Zeit
	- [ ] highscore in json speichern
	- [x] hindernisse vorne entfernen und hinten neue einfügen statt konstant 50, die ausgehen