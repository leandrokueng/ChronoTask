using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ChronoTask
{
    public class ChronoTaskApp
    {
        private readonly IProjectRepository _repo;
        private readonly ILogger<ChronoTaskApp> _log;
        private List<Project> _projects = new();

        public ChronoTaskApp(IProjectRepository repo, ILogger<ChronoTaskApp> log)
        {
            _repo = repo;
            _log = log;
        }

        public async Task RunAsync()
        {
            _projects = await _repo.LoadAsync();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== ChronoTask ===");
                Console.WriteLine("1. Projekte anzeigen");
                Console.WriteLine("2. Projekt anlegen");
                Console.WriteLine("3. Projekt löschen");
                Console.WriteLine("4. Aufgabe anlegen");
                Console.WriteLine("5. Aufgabenstatus ändern");
                Console.WriteLine("6. Aufgabe löschen");
                Console.WriteLine("7. Speichern & Beenden");
                Console.Write("Auswahl: ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1": ListProjects(); break;
                    case "2": AddProject(); break;
                    case "3": DeleteProject(); break;
                    case "4": AddTask(); break;
                    case "5": ChangeTaskStatus(); break;
                    case "6": DeleteTask(); break;
                    case "7":
                        await _repo.SaveAsync(_projects);
                        _log.LogInformation("Gespeichert {Count} Projekte", _projects.Count);
                        Console.WriteLine("Gespeichert. Bis bald!");
                        return;
                    default:
                        Console.WriteLine("Ungültige Eingabe …");
                        Pause();
                        break;
                }
            }
        }

        
        private void ListProjects()
        {
            Console.Clear();
            Console.WriteLine("--- Projekte ---\n");
            if (_projects.Count == 0)
                Console.WriteLine("(keine Projekte vorhanden)");
            else
                for (int i = 0; i < _projects.Count; i++)
                    Console.WriteLine($"{i + 1}. {_projects[i]}");
            Pause();
        }

        private void AddProject()
        {
            Console.Clear();
            Console.Write("Neuer Projektname: ");
            var name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name)) return;

            _projects.Add(new Project { Name = name.Trim() });
            _log.LogDebug("Projekt {Name} angelegt", name);
            Console.WriteLine("Projekt angelegt.");
            Pause();
        }

        private void DeleteProject()
        {
            if (!EnsureProjectExists()) return;

            Console.Clear();
            Console.WriteLine("--- Projekt löschen ---\n");
            var proj = SelectProject();
            if (proj == null) return;

            _projects.Remove(proj);
            _log.LogDebug("Projekt {Name} gelöscht", proj.Name);
            Console.WriteLine($"Projekt »{proj.Name}« gelöscht.");
            Pause();
        }

        
        private void AddTask()
        {
            if (!EnsureProjectExists()) return;

            Console.Clear();
            var proj = SelectProject();
            if (proj == null) return;

            Console.Write("Task-Titel: ");
            var title = Console.ReadLine();
            Console.Write("Beschreibung: ");
            var desc = Console.ReadLine();

            DateTime due;
            while (true)
            {
                Console.Write("Fälligkeitsdatum (yyyy-MM-dd): ");
                var input = Console.ReadLine();
                if (DateTime.TryParse(input, out due) &&
                    due.Date >= DateTime.Today)
                    break;

                Console.WriteLine("Ungültiges Datum – muss heute oder in Zukunft liegen.");
            }

            proj.Tasks.Add(new TaskItem
            {
                Title = title ?? string.Empty,
                Description = desc,
                DueDate = due
            });
            _log.LogDebug("Task {Title} zu Projekt {Project} hinzugefügt", title, proj.Name);
            Console.WriteLine("Task angelegt.");
            Pause();
        }

        private void ChangeTaskStatus()
        {
            if (!EnsureProjectExists()) return;

            Console.Clear();
            var proj = SelectProject();
            if (proj == null || proj.Tasks.Count == 0)
            {
                Console.WriteLine("Keine Aufgaben vorhanden.");
                Pause();
                return;
            }

            Console.WriteLine("--- Aufgaben ---\n");
            for (int i = 0; i < proj.Tasks.Count; i++)
                Console.WriteLine($"{i + 1}. {proj.Tasks[i]}");

            Console.Write("Aufgabe wählen: ");
            if (!int.TryParse(Console.ReadLine(), out int idx) ||
                idx < 1 || idx > proj.Tasks.Count) return;

            var task = proj.Tasks[idx - 1];

            Console.Write("Neuer Status (0=Todo,1=InProgress,2=Done): ");
            if (!Enum.TryParse<TaskStatus>(Console.ReadLine(), out var status)) return;

            task.Status = status;
            _log.LogDebug("Status von Task {Title} -> {Status}", task.Title, status);
            Console.WriteLine("Status aktualisiert.");
            Pause();
        }

        private void DeleteTask()
        {
            if (!EnsureProjectExists()) return;

            Console.Clear();
            var proj = SelectProject();
            if (proj == null || proj.Tasks.Count == 0)
            {
                Console.WriteLine("Keine Aufgaben vorhanden.");
                Pause();
                return;
            }

            Console.WriteLine("--- Aufgaben ---\n");
            for (int i = 0; i < proj.Tasks.Count; i++)
                Console.WriteLine($"{i + 1}. {proj.Tasks[i]}");

            Console.Write("Aufgabe löschen (Nummer): ");
            if (!int.TryParse(Console.ReadLine(), out int idx) ||
                idx < 1 || idx > proj.Tasks.Count) return;

            var removed = proj.Tasks[idx - 1];
            proj.Tasks.RemoveAt(idx - 1);
            _log.LogDebug("Task {Title} gelöscht", removed.Title);
            Console.WriteLine("Aufgabe gelöscht.");
            Pause();
        }

        
        private Project? SelectProject()
        {
            Console.WriteLine("Projekt wählen:");
            for (int i = 0; i < _projects.Count; i++)
                Console.WriteLine($"{i + 1}. {_projects[i].Name}");
            Console.Write("Nummer: ");

            return int.TryParse(Console.ReadLine(), out int idx) &&
                   idx > 0 && idx <= _projects.Count
                   ? _projects[idx - 1]
                   : null;
        }

        private bool EnsureProjectExists()
        {
            if (_projects.Count != 0) return true;
            Console.WriteLine("Bitte zuerst ein Projekt anlegen!");
            Pause();
            return false;
        }

        private static void Pause()
        {
            Console.WriteLine("Weiter mit Enter …");
            Console.ReadLine();
        }
    }
}
