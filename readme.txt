Windows 7...
Für Webzugriff ist folgendes zu tun:
* Port 80 (HTTP) freimachen: Programm liefert Fehlermeldung, wenn Port nicht frei ist
   * Ursachen: Windows-Dienst (z.B. IIS oder "WWW-Publishing-Dienst") --> beenden, deaktivieren
* Firewall konfigurieren, so dass eingehende Verbindungen über Port 80 zulässig sind
   * Systemsteuerung -> Firewall -> "Ein Programm oder Feature durch die Windows-Firewall zulassen"
   --> Port 80 (eingehend) zulassen
