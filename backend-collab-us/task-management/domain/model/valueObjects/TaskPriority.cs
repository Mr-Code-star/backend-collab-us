namespace backend_collab_us.task_management.domain.model.valueObjects;

public class TaskPriority
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public int Level { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public static readonly string LOW = "low";
    public static readonly string MEDIUM = "medium";
    public static readonly string HIGH = "high";
    public static readonly string URGENT = "urgent";
}