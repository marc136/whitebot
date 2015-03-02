---
layout: post
title:  Das Programm LearnBenchmark
date:   2015-01-17
categories: learning, controller
---

##Ziel
Die aktuelle Implementierung des Machine Learning Controllers ist so konfiguriert, dass eine Anzahl an zu lernenden Winkeln vorgegeben wird, und diese nacheinander durchlaufen werden.  
Nach jeder Iteration (Generation) werden die Ergebnisse in einer .json Datei gespeichert. Diese kann auch von einem Machine Learning Controller wieder eingelesen werden, sodass verschiedene Konfigurationen ausprobiert werden können.  

Um jedoch den Fortschritt in den einzelnen Generationen ansprechend auszugeben, wurde das WhiteBot LearnTool geschrieben.

##WhiteBotLearnTool
Dieses Programm existiert in der Extra-Solution **LearnBenchmark** im Ordner **LearnTool**.  
Die Werte, die beim Lernen der Winkel verwendet werden und in Dateien geschrieben werden (Winkel, Konfiguration des Linear und Angular speed, sowie die resultierende Abweichung vom Zielpunkt), werden von diesem Programm eingelsesen.  

Nach Start des LearnTools kann man einen Ordner auswählen. Hier sollte der ausgewählt werden, in den der aktuelle Machine Learning Controller seine Daten schreibt. Dieser Ordner wird fortan überwacht, und alle JSON Dateien werden eingelesen.  

Ausgegeben werden dann die Generation und die durchschnittliche Abweichung der besten bekannten Konfiguration für die Winkel.  
Dadurch lässt sich gut verfolgen, wann es an der Zeit ist, das Lernen des Controllers abzubrechen. So kann man davon ausgehen, dass nach fünf Generationen mit minimalen Veränderungen auch nach der sechsten kaum eine Änderung mehr stattfinden wird.  

![Learntool]({{site.baseurl}}/assets/learntool.png)


Eine Log-Datei des Machine Learning Controllers im JSON-Datenformat, in der nur vier Winkel und 2 Generationen getestet wurden, sieht zum Beispiel so aus:

{% highlight json %}
{
  "learnedAngles": {
    "1, 0": {
      "triedConfigurations": [
        {
          "LinearSpeed": 100,
          "AngularSpeed": 0,
          "Deviation": -3.923583984375
        },
        {
          "LinearSpeed": 100,
          "AngularSpeed": 100,
          "Deviation": 34.381317138671875
        }
      ]
    },
    "0, 1": {
      "triedConfigurations": [
        {
          "LinearSpeed": 100,
          "AngularSpeed": 0,
          "Deviation": -1.6170654296875
        },
        {
          "LinearSpeed": 100,
          "AngularSpeed": 100,
          "Deviation": 6.8730010986328125
        }
      ]
    },
    "-1, 0": {
      "triedConfigurations": [
        {
          "LinearSpeed": 100,
          "AngularSpeed": 100,
          "Deviation": 23.632476806640625
        },
        {
          "LinearSpeed": 100,
          "AngularSpeed": 0,
          "Deviation": 40.027603149414062
        }
      ]
    },
    "0, -1": {
      "triedConfigurations": [
        {
          "LinearSpeed": 100,
          "AngularSpeed": 0,
          "Deviation": 7.797027587890625
        },
        {
          "LinearSpeed": 100,
          "AngularSpeed": 100,
          "Deviation": 42.200515747070312
        }
      ]
    }
  }
}
{% endhighlight %}

Die probierten Konfigurationen sind aufsteigend nach der Abweichung sortiert, sodass die beste bekannte Konfiguration immer als erste eingelesen werden kann.  


###Ordner im Dateisystem überwachen
Das Dateisystem wird nicht gepollt, sondern die System-Events zum Erzeugen von Dateien werden abgefangen.  
Dafür wird das Objekt **FileSystemWatcher** (verfügbar ab .NET 4.0) verwendet.
