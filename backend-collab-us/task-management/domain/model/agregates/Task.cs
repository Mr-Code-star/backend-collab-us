using backend_collab_us.projects.domain.model.agregates;
using backend_collab_us.task_management.domain.model.commands;
using backend_collab_us.task_management.domain.model.valueObjects;
using TaskStatus = backend_collab_us.task_management.domain.model.valueObjects.TaskStatus;
using TaskPriority = backend_collab_us.task_management.domain.model.valueObjects.TaskPriority;


namespace backend_collab_us.task_management.domain.model.agregates;

public partial class Task
{
    public int Id { get; private set; }
    public string Title { get; set; } = string.Empty;
    public string Description {get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public string Status { get; set; } = TaskStatus.PENDING;
    public string Priority { get; set; } = TaskPriority.MEDIUM;
    public int ProjectId { get; private set; }

    public Project Project { get; private set;}
    public int AssignedTo {get; set; } // collaboratorid
    public string AssignedToName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
    public int Progress { get; private set; }
    public int EstimatedHours { get; set; }
    public int ActualHours { get; set; }
    public int CreatedBy { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? CompletedAt { get; private set; }
    
    // Value Objects Collections
    
    // Checklist
    private readonly List<ChecklistItem> _checklist = new();
    public IReadOnlyCollection<ChecklistItem> Checklist => _checklist.AsReadOnly();
    // Tools
    private readonly List<TaskTool> _tools = new();
    public IReadOnlyCollection<TaskTool> Tools => _tools.AsReadOnly();
    // Attachments
    private readonly List<TaskAttachment> _attachments = new();
    public IReadOnlyCollection<TaskAttachment> Attachments => _attachments.AsReadOnly();

    protected Task()
    {
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }
    
    public Task(CreateTaskCommand command)
    {
        Title = command.Title;
        Description = command.Description;
        DueDate = command.DueDate;
        Status = command.Status ?? TaskStatus.PENDING;
        Priority = command.Priority ?? TaskPriority.MEDIUM;
        ProjectId = command.ProjectId;
        AssignedTo = command.AssignedTo;
        AssignedToName = command.AssignedToName;
        Role = command.Role;
        Comment = command.Comment ?? string.Empty;
        EstimatedHours = command.EstimatedHours;
        ActualHours = 0;
        CreatedBy = command.CreatedBy;
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;

        // Agregar checklist items
        if (command.Checklist != null)
        {
            foreach (var itemCommand in command.Checklist)
            {
                var checklistItem = new ChecklistItem(itemCommand.Text, itemCommand.Completed);
                _checklist.Add(checklistItem);
            }
        }

        // Agregar tools
        if (command.Tools != null)
        {
            foreach (var toolCommand in command.Tools)
            {
                var tool = new TaskTool(toolCommand.Name, toolCommand.Checked);
                _tools.Add(tool);
            }
        }

        // Agregar attachments
        if (command.Attachments != null)
        {
            foreach (var attachmentCommand in command.Attachments)
            {
                var attachment = new TaskAttachment(
                    attachmentCommand.Name,
                    attachmentCommand.Type,
                    attachmentCommand.Url,
                    attachmentCommand.Icon
                );
                _attachments.Add(attachment);
            }
        }

        UpdateProgress();
        UpdateStatusBasedOnDueDate();
    }
    // Business Logic Methods
    public void UpdateProgress()
    {
        if (_checklist.Count == 0)
        {
            Progress = 0;
            return;
        }

        var completedItems = _checklist.Count(item => item.Completed);
        Progress = (int)Math.Round((double)completedItems / _checklist.Count * 100);

        // Auto-complete if all checklist items are done
        if (Progress == 100 && Status != TaskStatus.COMPLETED)
        {
            MarkAsCompleted();
        }

        UpdatedAt = DateTime.Now;
    }
    public void MarkAsCompleted()
    {
        Status = TaskStatus.COMPLETED;
        Progress = 100;
        CompletedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;

        // Mark all checklist items as completed
        foreach (var item in _checklist)
        {
            if (!item.Completed)
            {
                item.MarkAsCompleted();
            }
        }
    }
    public void UpdateStatusBasedOnDueDate()
    {
        if (Status == TaskStatus.COMPLETED) return;

        var now = DateTime.Now;

        if (DueDate < now)
        {
            Status = TaskStatus.DELAYED;
        }
        else if (Status == TaskStatus.DELAYED && DueDate >= now)
        {
            Status = TaskStatus.PENDING;
        }

        UpdatedAt = DateTime.Now;
    }
    
    /// <summary>
    /// Actualiza la fecha de vencimiento de la tarea
    /// </summary>
    public void UpdateDueDate(UpdateTaskDueDateCommand command)
    {
        if (command.NewDueDate < DateTime.Now)
            throw new ArgumentException("La nueva fecha no puede ser en el pasado");

        DueDate = command.NewDueDate;
        UpdatedAt = DateTime.Now;
    
        // Re-evaluar el estado basado en la nueva fecha
        UpdateStatusBasedOnDueDate();
    }
    
    /// <summary>
    /// Procesa si la tarea está vencida y actualiza su estado
    /// </summary>
    public void ProcessOverdueStatus(ProcessOverdueTasksCommand command)
    {
        // Solo procesar si no está completada
        if (Status != TaskStatus.COMPLETED)
        {
            var now = command.CurrentDate;
        
            if (DueDate < now && Status != TaskStatus.DELAYED)
            {
                // La tarea está vencida y no estaba marcada como retrasada
                Status = TaskStatus.DELAYED;
                UpdatedAt = DateTime.Now;
            }
            else if (DueDate >= now && Status == TaskStatus.DELAYED)
            {
                // La tarea ya no está vencida (quizás se actualizó la fecha)
                Status = TaskStatus.PENDING;
                UpdatedAt = DateTime.Now;
            }
        }
    }
    /// <summary>
    /// Valida si la tarea está vencida según una fecha de referencia
    /// </summary>
    public bool IsOverdue(DateTime referenceDate)
    {
        return Status != TaskStatus.COMPLETED && DueDate < referenceDate;
    }
}