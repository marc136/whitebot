---
layout: post
title:  Struktur | WhiteBot
date:   2014-11-04
categories: controller structure
---

##Die Solution WhiteBot
befindet sich im Ordner WhiteBot und enthält verschiedene Unterprojekte:  

###GCodeParser
Enthält Code der verwendet wird um GCodes zu verarbeiten und kann dadurch leicht aus der Anwendung herausgelöst werden und an anderer Stelle wiederverwendet werden.  
Die Funktionen sind in einem [eigenen Post]({{site.baseurl}}/gcode/g-code/2014/12/20/gcode.html)] erklärt.  

###Logging
Enthält das Interface ILogger und eine Beispielimplementierung eines asynchron schreibenden dateibasierten Loggers.  
Mehr dazu steht in einem [eigenen Post]({{site.baseurl}}/logging/2014/11/10/logging.html)].  

###Sensors
Enthält Klassen um den Blidgets controller anzusprechen, Sensoren auszulesen und Gravitations- und Ausrichtungs-Informationen zu verarbeiten.  

###Tests
Enthält zur Zeit nur einen Test, dass das Objekt LearnAngleResult korrekt auf die Festplatte geschrieben und wieder von dieser gelesen werden kann.  
Verwendet wird das Unit Testing Framework von Visual Studio (Ähnlich zu XUnit).  

###WhiteBot
Enthält die wichtigsten Komponenten, die in drei Ordner unterteilt sind:  
_LowLevel_ enthält Klassen für die Ansteuerung der verschiedenen Motoren.  
_AbstractComponents_ besteht aus der Kommando-Klasse für Richtungs- sowie Stift- und Radierer-Kommandos. Diese werden in den ebenfalls darin enthaltenen _Pathplanner_ und _PathplannerSamples_ verwendet und kommen auch beim GCode-Parser zum Einsatz.  
Pathplanner-Instanzen werden verwendet um die RoboterController bestimmte Aufgaben ausführen zu lassen.  
_RobotController_ kennt die abstrakte Klasse AbstractRobotController, von der der Machine-Learning-Controller und der PID-Controller ableiten. Zudem gibt es noch einen GamepadController.  

###WinFormsApp
Dieses Projekt ist der Einstiegspunkt der Anwendung und enthält den kompletten Code für die Windows Forms Anwendung, die dem Benutzer angezeigt wird.  
Wenn die Anwendung auf WPF, GTK# oder ein anderes System umgestellt werden sollte, muss nur ein neues Projekt erstellt werden und das bauen des Projekts WinFormsApp deaktiviert werden.  

Enthalten sind die _LandingPage_ und die _MainForm_:  
Auf der _Landing Page_ kann ausgewählt werden, ob der PID-Controller oder ein Machine-Learning-Controller gestartet werden soll, oder ob ein bereits gelernter Machine-Learning-Controller aus einer Datei gelesen werden soll.  
Die _Main Form_ erlaubt die komfortable Steuerung des Roboters, so wird die Position des Roboters angezeigt, bestimmte Punkte können durch Maus-Klick angefahren werden und vorgegebene Abfolgen wie ein Stern oder eine Sinus-Kurve können gestartet werden. Auch das Lernen aller Winkel in einer Generation kann hier gestartet werden.  

