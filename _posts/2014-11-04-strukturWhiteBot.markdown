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
###Tests
Enthält zur Zeit nur einen Test, dass das Objekt LearnAngleResult korrekt auf die Festplatte geschrieben und wieder von dieser gelesen werden kann.  
Verwendet wird das Unit Testing Framework von Visual Studio (Ähnlich zu XUnit).  

###WhiteBot

###WinFormsApp
Dieses Projekt ist der Einstiegspunkt der Anwendung und enthält den kompletten Code für die Windows Forms Anwendung, die dem Benutzer angezeigt wird.  
Wenn die Anwendung auf WPF, GTK# oder ein anderes System umgestellt werden sollte, muss nur ein neues Projekt erstellt werden und das bauen des Projekts WinFormsApp deaktiviert werden.  



---
#Beispiel Code lighlighting
{% highlight c# %}
var i = 1;
{% endhighlight %}

Code ohne Highlighting  

  2014-11-10  10:22:40:561  start
  2014-11-10  10:22:40:661  1=1; 2=zwei
