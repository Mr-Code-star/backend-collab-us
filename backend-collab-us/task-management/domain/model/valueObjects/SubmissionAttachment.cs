namespace backend_collab_us.task_management.domain.model.valueObjects;

public class SubmissionAttachment
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Type { get; private set; } = string.Empty; // 'file', 'image', 'document', etc.
    public string Url { get; private set; } = string.Empty;
    public long Size { get; private set; } // in bytes
    public DateTime UploadedAt { get; private set; }

    public int TaskSubmissionId { get; private set; } 
    public SubmissionAttachment(string name, string type, string url, long size)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Attachment name is required");

        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("Attachment URL is required");

        if (size <= 0)
            throw new ArgumentException("Attachment size must be positive");

        Name = name;
        Type = type;
        Url = url;
        Size = size;
        UploadedAt = DateTime.Now;
    }

    protected SubmissionAttachment() { }
}