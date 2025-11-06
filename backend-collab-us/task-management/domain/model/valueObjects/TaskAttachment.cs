namespace backend_collab_us.task_management.domain.model.valueObjects;

public class TaskAttachment
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Type { get; private set; } = "file"; // 'file' or 'link'
    public string Url { get; private set; } = string.Empty;
    public string Icon { get; private set; } = "pi pi-file";
    public DateTime UploadedAt { get; private set; }
    public int? TaskId { get; private set; }

    protected TaskAttachment()
    {
        UploadedAt = DateTime.Now;
    }

    public TaskAttachment(string name, string type, string url, string icon = null, int? taskId = null)
    {
        Name = name;
        Type = type;
        Url = url;
        Icon = icon ?? (type == "link" ? "pi pi-link" : "pi pi-file");
        UploadedAt = DateTime.Now;
        TaskId = taskId;
    }
}