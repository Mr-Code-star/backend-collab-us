using backend_collab_us.IAM.domain.model.agregates;
using backend_collab_us.projects.domain.model.commands;
using backend_collab_us.projects.domain.model.valueObjects;

using Task = backend_collab_us.task_management.domain.model.agregates.Task;

namespace backend_collab_us.projects.domain.model.agregates;

public partial class Project
{
   public int Id { get; private set; } // Id automatico
   public int UserId { get; private set; }
   
   public User User { get; private set; }
   public string Title { get; private set; }
   public string Description { get; private set; }
   public string Summary { get; private set; }
   public AcademicLevel AcademicLevelName { get; private set; }
   public string Benefits { get; private set; }
   public List<string> Skills { get; private set; } // Arreglo
   public int DurationQuantity { get; private set; }
   public DurationType DurationType { get; private set; }
   public string Status { get; private set; } = "draft";
   public int Progress { get; private set; }
   public DateTime CreatedAt { get; private set; }
   public DateTime UpdatedAt { get; private set; }
   // ROL
   private readonly List<Rol> _roles = new();
   public IReadOnlyCollection<Rol> Roles => _roles.AsReadOnly();
   // Areas
   protected internal List<string> _areas = new();
   public IReadOnlyCollection<string> Areas => _areas.AsReadOnly();
  // Colaboradores
  
  public virtual List<Application> Collaborators { get; private set; } = new();
  // Tags
  protected internal List<string> _tags = new();
  public IReadOnlyCollection<string> Tags => _tags.AsReadOnly();
  
  private readonly List<Task> _tasks = new();
  public IReadOnlyCollection<Task> Tasks => _tasks.AsReadOnly();
  
  protected Project()
  {
      Title = string.Empty;
      Description = string.Empty;
      Summary = string.Empty;
      Benefits = string.Empty;
      Skills = new List<string>();
      Status = "draft";
      Progress = 0;
      CreatedAt = DateTime.Now;
      UpdatedAt = DateTime.Now;
  }

  public Project(
      int userId,
      string title,
      string description,
      string summary,
      AcademicLevel academicLevelName,
      string benefits,
      List<string> skills,
      int durationQuantity,
      DurationType durationType,
      List<string> areas,
      List<string> tags,
      string status = "draft",
      int progress = 0)
  {
      UserId = userId;
      Title = title;
      Description = description;
      Summary = summary;
      AcademicLevelName = academicLevelName;
      Benefits = benefits;
      Skills = skills ?? new List<string>();
      DurationQuantity = durationQuantity;
      DurationType = durationType;
      Status = status;
      Progress = progress;
      CreatedAt = DateTime.Now;
      UpdatedAt = DateTime.Now;

      // Agregar áreas
      if (areas != null)
      {
          _areas.AddRange(areas);
      }

      // Agregar tags
      if (tags != null)
      {
          _tags.AddRange(tags);
      }
  }
  
  public Project(CreateProjectCommand command)
    {
      UserId = command.UserId;
      Title = command.Title;
      Description = command.Description;
      Summary = command.Summary;
      AcademicLevelName = command.AcademicLevelName;
      Benefits = command.Benefits;
      Skills = command.Skills ?? new List<string>();
      DurationQuantity = command.DurationQuantity;
      DurationType = command.DurationType;
      Status = command.Status;
      Progress = command.Progress;
      CreatedAt = DateTime.Now;
      UpdatedAt = DateTime.Now;

      // Agregar áreas
      if (command.Areas != null)
      {
          _areas.AddRange(command.Areas);
      }

      // Agregar tags
      if (command.Tags != null)
      {
          _tags.AddRange(command.Tags);
      }

      // Agregar roles desde el comando
      if (command.Roles != null)
      {
          foreach (var roleCommand in command.Roles)
          {
              var role = new Rol(roleCommand, Id); // El Id del proyecto se establecerá después
              _roles.Add(role);
          }
      }
    }
    public void AddTask(Task task)
    {
        if (task.ProjectId != Id)
            throw new InvalidOperationException("La tarea no pertenece a este proyecto");

        _tasks.Add(task);
        UpdatedAt = DateTime.Now;
        
        // Opcional: Recalcular progreso del proyecto
        UpdateProjectProgress();
    }
    public bool RemoveTask(int taskId)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == taskId);
        if (task != null)
        {
            _tasks.Remove(task);
            UpdatedAt = DateTime.Now;
            UpdateProjectProgress();
            return true;
        }
        return false;
    }
    private void UpdateProjectProgress()
    {
        if (_tasks.Count == 0)
        {
            Progress = 0;
            return;
        }

        var totalProgress = _tasks.Sum(t => t.Progress);
        Progress = totalProgress / _tasks.Count;
    }

}