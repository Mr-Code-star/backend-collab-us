namespace backend_collab_us.task_management.domain.model.valueObjects;

public class TaskTool
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public bool Checked { get; private set; }
    public int? TaskId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    protected TaskTool()
    {
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }

    public TaskTool(string name, bool isChecked = false, int? taskId = null)
    {
        Name = name;
        Checked = isChecked;
        TaskId = taskId;
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }

    public void Toggle()
    {
        Checked = !Checked;
        UpdatedAt = DateTime.Now;
    }

    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Tool name cannot be empty");

        Name = newName.Trim();
        UpdatedAt = DateTime.Now;
    }
}