---
layout: post
title:  Abhängigkeiten | Roboter Controller
date:   2014-10-30 
categories: dependencies
---

Die Solution dieses Projektes ist der Controller des Roboters, der die Steuerung übernimmt.
Damit alles richtig funktioniert, muss zuerst der Roboter mit Bluetooth verbunden, die Kamera richtig ausgerichtet und das Programm TouchServer aus der Solution TouchTable gestartet werden.  

Was nötig ist, um TouchTable zu kompilieren, wurde in einem eigenen Blog Post beschrieben.


##Abhängigkeiten
Die Solution WhiteBot lässt sich nur auf einem Rechner mit Windows 8 kompilieren, da Windows 7 noch nicht die benötigten Treiber für Bluetooth 4.0 LE (Low Energy) mitbringt.

Für den RoboterController wird noch das [XNA-Framework][XNA Redistributable] benötigt (sowohl für Vektoren als auch für die Steuerung mit dem Gamepad). Das [Redistributable][XNA Redistributable] reicht aber aus.  


##Weitere Informationen
Die Oberfläche ist mit Windows Forms erstellt.  
Weitere Informationen zur Struktur sind im Post [Struktur | Roboter Controller]({{site.baseurl}}/controller/structure/2014/11/04/strukturWhiteBot.html)


##Erstes Ausführen
Wie schon beschrieben muss der Roboter zuerst eingeschalten werden (sowohl blidget als auch die Motoren).  
Danach muss eine Bluetooth Verbindung aufgebaut werden.  
Danach lässt sich das Programm starten.


[XNA Redistributable]: http://www.microsoft.com/en-us/download/details.aspx?id=20914
