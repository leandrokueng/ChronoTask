using System;
using System.Collections.Generic;

namespace ChronoTask
{
    public enum TaskStatus { Todo, InProgress, Done }

    public class TaskItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public TaskStatus Status { get; set; } = TaskStatus.Todo;

        public bool IsOverdue =>
            Status != TaskStatus.Done && DueDate.Date < DateTime.Today;

        public override string ToString() =>
            $"{(IsOverdue ? "!!! " : string.Empty)}" +
            $"[{Status}] {Title} (fällig: {DueDate:yyyy-MM-dd})";
    }

    public class Project
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public List<TaskItem> Tasks { get; set; } = new();

        public override string ToString()
        {
            int done = Tasks.FindAll(t => t.Status == TaskStatus.Done).Count;
            return $"{Name} (Tasks: {Tasks.Count}, erledigt: {done})";
        }
    }
}
