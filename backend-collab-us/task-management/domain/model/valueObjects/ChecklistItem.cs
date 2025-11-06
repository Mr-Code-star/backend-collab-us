namespace backend_collab_us.task_management.domain.model.valueObjects;

public class ChecklistItem
{
    public int Id { get; private set; }
    public string Text { get; private set; } = string.Empty;
    public bool Completed { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public int? TaskId { get; private set; }

    protected ChecklistItem()
    {
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }

    public ChecklistItem(string text, bool completed = false, int? taskId = null)
    {
        Text = text;
        Completed = completed;
        TaskId = taskId;
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }

    public void MarkAsCompleted()
    {
        Completed = true;
        UpdatedAt = DateTime.Now;
    }

    public void MarkAsPending()
    {
        Completed = false;
        UpdatedAt = DateTime.Now;
    }

    public void UpdateText(string newText)
    {
        if (string.IsNullOrWhiteSpace(newText))
            throw new ArgumentException("Checklist item text cannot be empty");

        Text = newText.Trim();
        UpdatedAt = DateTime.Now;
    }
}