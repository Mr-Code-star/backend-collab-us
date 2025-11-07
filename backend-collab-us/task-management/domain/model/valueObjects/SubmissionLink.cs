namespace backend_collab_us.task_management.domain.model.valueObjects;

public class SubmissionLink
{
    public int Id { get; private set; }
    public string Url { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    public int TaskSubmissionId { get; private set; }
    public SubmissionLink(string url, string description = "")
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("URL is required");

        if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            throw new ArgumentException("URL must be a valid absolute URL");

        Url = url;
        Description = description ?? string.Empty;
        CreatedAt = DateTime.Now;
    }

    protected SubmissionLink() { }
}