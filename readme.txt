Windows 7...
F�r Webzugriff ist folgendes zu tun:
* Port 80 (HTTP) freimachen: Programm liefert Fehlermeldung, wenn Port nicht frei ist
   * Ursachen: Windows-Dienst (z.B. IIS oder "WWW-Publishing-Dienst") --> beenden, deaktivieren
* Firewall konfigurieren, so dass eingehende Verbindungen �ber Port 80 zul�ssig sind
   * Systemsteuerung -> Firewall -> "Ein Programm oder Feature durch die Windows-Firewall zulassen"
   --> Port 80 (eingehend) zulassen
