namespace backend_collab_us.task_management.domain.model.valueObjects;

public class TaskStatus
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public static readonly string PENDING = "pending";
    public static readonly string COMPLETED = "completed";
    public static readonly string DELAYED = "retrasado";
}