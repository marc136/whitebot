#WhiteBot LogTool

##Die Logfiles
Es gibt 3 .log Dateien pro Controller. Zum Beispiel:

###robot0_controller
Enthält Logs, die von RobotController geschrieben werden.  
_Implementiert im AbstractRobotController._  

Format der Zeilen:  
Timestamp | RobotControllerName | Befehl  
_RobotControllerName_ ist entweder [ RobotControllerAbstract, RobotControllerPID, RobotControllerML ]  
Der _Befehl_ kann zusätzlich noch in Klammern Zusatzinformationen enthalten, zum Beispiel:  
```Rotate(angle: -0,818822292975193 | angular: 943,790162763996)```  
oder  
```Stop(reason: path was finished)```  

###robot0_pathplanner
Enthält Logs, die vom Pathplanner geschrieben werden.  

Format der Zeilen:  
```
Timestamp | "PathPlanner" | Informationen  
oder  
Timestamp | "PathPlanner" | "Controller:" | "Start task "[taskname](taskInformation)  
oder  
Timestamp | "PathPlanner" | "Controller:" | "Finished task "[taskname]  
```

Beispiel:  
```
14:28:33:650	PathPlanner	Start session
14:29:18:630	PathPlanner	Controller:	start task	LookAt(x:180|y:100)
14:29:19:860	PathPlanner	Controller:	Finished task LookAt
```  

###robot0_log:
Enthält die Positionsdaten mit Tabs getrennt zu jedem Tick.  
_Implementiert im AbstractRobotController._

Format der Zeilen:  
```
Timestamp | Position.X | Position.Y | LookDirection.X | LookDirection.Y | Gravity.X | Gravity.Y | Gravity.Z | DirectionVector.X | DirectionVector.Y  
```

##Ablauf
Das Pathplanner-Logfile wird einmal komplett gelesen und die Timestamps aller Einträge _Start session_ werden gespeichert.  
Beispiel:  
```14:28:33:650	PathPlanner	Start session```

Diese werden dem Benutzer in einer Liste angezeigt.  
Der Benutzer kann dann auf ein Element klicken und den Button _auswerten_ anwählen.  
Dadurch wird das Logfile noch einmal gelesen und alle Zeilen, die sich zwischen dem _Start session_ dieses Timestamps und dem nächsten _Start session_ (oder einem _Stop session_ oder dem Dateiende) befinden werden weiterverarbeitet.  

Alle Zeilen, die ein _MoveTo_ enthalten werden herausgepickt und weiter verarbeitet.  

Für jede zwei Zeilen mit MoveTo (start und finish)
Der Timestamp mit dem Beginn des Tasks und mit dem Ende des Tasks werden gespeichert.