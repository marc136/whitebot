---
layout: post
title:  "Abhängigkeiten | Touchtable"
date:   2014-10-29
categories: dependencies
---
Die Daten aus der Kamera werden von der Anwendung TouchTable per UDP über eine HTML-Adresse versendet.  
Um TouchTable kompilieren und ausführen zu können, müssen einige Abhängigkeiten erfüllt werden.  

##Grundsätzlich
Wichtig in der Solution TouchTable sind vor allem die zwei Projekte **TouchVisualizer** und **TouchServer**.  
Wird das erste gestartet, lässt sich überprüfen, was die Kamera wirklich sieht und welche Punkte erkannt werden. Nach dem Start des zweiten werden die erkannten Punkte mittels UDP-Pakete an eine zu übergebende IP Adresse versendet.  
Zuerst sollte **TouchVisualizer** ausprobiert werden, und danach wird eigentlich immer **TouchServer** verwendet.  

##Abhängigkeiten und Bibliotheken
Wenn die Solution zum ersten Mal geöffnet wird, und man versucht ein Projekt daraus zu kompilieren, stößt man auf viele Fehler, die auf Abhängigkeiten zurückzuführen sind und die behoben werden müssen.

###OpenNI 2
Zuerst muss [OpenNI 2](http://structure.io/openni) installiert werden, dabei handelt es sich um ein Open Source SDK und Treiber für verschiedene Tiefenkameras für Windows, Linux und Mac.  
Wenn die Installation geklappt hat, werden die folgenden Umgebungsvariablen gesetzt. ![Umgebungsvariablen]({{site.baseurl}}/assets/dependencies/001.png)  
Auch wenn die 32-Bit Version installiert wird, werden drei Variablen für den Include-, Library- und Redistributable Pfad gesetzt.  

###SDLpp
Danach treten immer noch einige Fehler auf. Zum Beispiel wird SDLpp nicht gefunden. ![SDLpp missing]({{site.baseurl}}/assets/dependencies/002.png)

In den Projekt Einstellungen im Unterpunkt Linker fällt beim Eintrag Additional Library Directories auf, dass _SDLpp_ in einem Verzeichnis gesucht wird, das über die _Benutzervariable **LIB_DIR**_ festgelegt wird. ![LIB_DIR]({{site.baseurl}}/assets/dependencies/003.png)

Diese Systemvariable muss gesetzt werden. Hierfür kann das Skript [LIB_DIR variable setzen.bat]({{site.baseurl}}/assets/LIB_DIR-variable-setzen.bat) verwendet werden, das diesen Wert auf den Unterordner _libraries_ des aktuellen Verzeichnisses setzt.  Diese ist nach einem Neustart verfügbar.  
_Hinweis:_ Zusätzlich wird auch eine Benutzervariable gesetzt, die in der aktuellen Anmeldesitzung direkt verwendet werden kann. Nach einem Neustart verschwindet diese und die Umgebungsvariable wird verwendet.  

Anschließend muss [SDLpp aus dem  Repository](https://github.com/patrigg/SDLpp) in den Library-Ordner geklont werden.



	cd %LIB_DIR%
	git clone https://github.com/patrigg/SDLpp

---

###PBL
In der Datei Lines _Lines.cpp_ im Projekt _TouchVisualizer_ wird außerdem noch pbl, eine weitere Bibliothek von Patrick, referenziert.  
![pbl]({{site.baseurl}}/assets/dependencies/008.png)  
Diese soll auch aus dem Verzeichnis **LIB_DIR** geladen werden. Deswegen wird das Projekt direkt dorthinein geklont.  

	cd %LIB_DIR%
	git clone https://github.com/patrigg/pbl

---

###Boost Lib
Das Projekt TouchServer, das die gefundenen Daten per UDP versendet, benötigt noch zusätzlich die [Boost-Bibliothek](http://www.boost.org/).  
Diese wird aus dem Verzeichnis **LIB_DIR** geladen und muss dorthin kopiert werden. Es wird direkt die Version 1.56.0 eingebunden, wie in Zeile vier der Datei _UdpSender.h_ zu sehen ist.  
![Einbindung der Boost Bibliothek]({{site.baseurl}}/assets/dependencies/007.png)  
Am Besten lädt man direkt die [vorkompilierte Version für die passende Visual Studio Version](http://sourceforge.net/projects/boost/files/boost-binaries/1.56.0/) herunter, da das Kompilieren sehr lange dauert.  


---

###SDL2
Danach fehlt eventuell noch die Bibliothek [SDL2](https://www.libsdl.org/download-2.0.php), um das Projekt bauen zu können.![SDL2]({{site.baseurl}}/assets/dependencies/004.png)  

In den Projekt Einstellungen ist zu sehen, dass der Ordner der SDL direkt verlinkt und je nach Prozessor angepasst wird.  
![SDL2 install]({{site.baseurl}}/assets/dependencies/005.png)  
Entweder kann das [Development Package von SDL 2](https://www.libsdl.org/download-2.0.php) direkt in den Ordner _C:\Program Files\SDL2_ installiert werden, oder der Pfad in den Projekteinstellungen muss entsprechend angepasst werden.  

---

##Ausprobieren
Das war es vorerst mit den Abhängigkeiten und das Projekt kann erfolgreich gebaut werden.
Zum Testen kann zuerst das Projekt _TouchVisualizer_ und danach _TouchServer_ gestartet werden.  
Um den TouchServer zu verwenden, müssen diesem beim Start IP-Adresse und Port übergeben werden, die verwendet werden sollen.  
Beim Debuggen in Visual Studio


##Erkennung verbessern
Wenn der Roboter nicht zuverlässig erkannt wird, kann es sein dass in der Datei TouchServer.cpp die Konstanten _ThresholdMin_, _ThresholdMax_ und _MinBlobSize_ angepasst werden müssen.  
![Konstanten zur Roboter-Erkennung]({{site.baseurl}}/assets/dependencies/006.png)  
Diese werden verwendet um Punkte zu erkennen und je nach Entfernung und Winkel der Kamera zum Whiteboard müssen diese angepasst werden.
