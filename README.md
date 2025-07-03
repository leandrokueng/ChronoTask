# ChronoTask

> **CLI-Projekt- und Aufgabenverwaltung auf .NET 8**  
> Minimalistisch, plattformunabhängig und erweiterbar – perfekt für kleine Teams oder persönliche To-do-Listen.

<br/>

## Highlights

| Feature | Beschreibung |
|---------|--------------|
| **Projekt- & Task-CRUD** | Legen Sie beliebig viele Projekte an, fügen Sie Aufgaben hinzu, ändern oder löschen Sie alles per Menü. |
| **Status-Workflow** | Aufgaben können **Todo → In Progress → Done** durchlaufen. |
| **Überfälligkeits-Check** | Tasks, deren Fälligkeitsdatum überschritten ist, werden mit „!!!“ markiert. |
| **JSON-Persistenz** | Alle Daten werden in `chronotask.json` gespeichert – keine Datenbank nötig. |
| **Clean Architecture** | Schichten – CLI / Domain / Repository – plus Dependency Injection & Logging mit Generic Host. |
| **Cross-Platform** | Läuft auf Windows, Linux, macOS – alles, was ein aktuelles .NET-Runtime hat. |

<br/>

## Schnellstart

```bash
# .NET 8 SDK erforderlich
git clone https://github.com/<DEIN_USER>/ChronoTask.git
cd ChronoTask/Bewerbungsprojekt
dotnet run
