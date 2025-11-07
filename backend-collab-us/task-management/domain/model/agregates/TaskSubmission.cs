using backend_collab_us.task_management.domain.model.commands;
using backend_collab_us.task_management.domain.model.valueObjects;

namespace backend_collab_us.task_management.domain.model.agregates;

public partial class TaskSubmission
{
    public int Id { get; private set; }
    public int TaskId { get; private set; }
    public int CollaboratorId { get; private set; }
 
    public string CollaboratorName { get; private set; } = string.Empty;

    public DateTime SubmittedAt { get; private set; }
    
    // Value Objects para la entrega de la tarea
    private readonly List<SubmissionLink> _links = new();
    public IReadOnlyCollection<SubmissionLink> Links => _links.AsReadOnly();
    
    private readonly List<SubmissionAttachment> _attachments = new();
    public IReadOnlyCollection<SubmissionAttachment> Attachments => _attachments.AsReadOnly();
    
    public string Notes { get; set; } = string.Empty;
    public string Status { get; private set; } = SubmissionStatus.SUBMITTED;

    // Revisión
    public DateTime? ReviewedAt { get; private set; }
    public int? ReviewerId { get; private set; }
    public string ReviewNotes { get; private set; } = string.Empty;
    
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // Relación con Task
    public Task Task { get; private set; }
    
    protected TaskSubmission()
    {
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }
    
    public TaskSubmission(CreateTaskSubmissionCommand command)
    {
        TaskId = command.TaskId;
        CollaboratorId = command.CollaboratorId;
        CollaboratorName = command.CollaboratorName;
        SubmittedAt = DateTime.Now;
        Notes = command.Notes ?? string.Empty;
        Status = SubmissionStatus.SUBMITTED;
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;

        // Agregar links
        if (command.Links != null)
        {
            foreach (var link in command.Links)
            {
                var submissionLink = new SubmissionLink(link.Url, link.Description);
                _links.Add(submissionLink);
            }
        }

        // Agregar attachments
        if (command.Attachments != null)
        {
            foreach (var attachmentCommand in command.Attachments)
            {
                var attachment = new SubmissionAttachment(
                    attachmentCommand.Name,
                    attachmentCommand.Type,
                    attachmentCommand.Url,
                    attachmentCommand.Size
                );
                _attachments.Add(attachment);
            }
        }

        ValidateSubmission();
    }
    
    // Business Logic Methods
    public void MarkAsReviewed(int reviewerId, string reviewNotes)
    {
        if (Status != SubmissionStatus.SUBMITTED)
        {
            throw new InvalidOperationException("Only submitted tasks can be reviewed");
        }

        Status = SubmissionStatus.REVIEWED;
        ReviewerId = reviewerId;
        ReviewNotes = reviewNotes;
        ReviewedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }
    
    public void RequestRevision(string revisionNotes)
    {
        if (Status != SubmissionStatus.SUBMITTED && Status != SubmissionStatus.REVIEWED)
        {
            throw new InvalidOperationException("Cannot request revision for this submission status");
        }

        Status = SubmissionStatus.NEEDS_REVISION;
        ReviewNotes = revisionNotes;
        UpdatedAt = DateTime.Now;
    }
    
    public void Resubmit(List<CreateSubmissionLinkCommand>? newLinks = null, 
        List<CreateSubmissionAttachmentCommand>? newAttachments = null,
        string? newNotes = null)
    {
        if (Status != SubmissionStatus.NEEDS_REVISION)
        {
            throw new InvalidOperationException("Only submissions needing revision can be resubmitted");
        }

        Status = SubmissionStatus.SUBMITTED;
        SubmittedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;

        // Actualizar notas si se proporcionan
        if (!string.IsNullOrEmpty(newNotes))
        {
            Notes = newNotes;
        }

        // Agregar nuevos links
        if (newLinks != null)
        {
            foreach (var link in newLinks)
            {
                var submissionLink = new SubmissionLink(link.Url, link.Description);
                _links.Add(submissionLink);
            }
        }

        // Agregar nuevos attachments
        if (newAttachments != null)
        {
            foreach (var attachmentCommand in newAttachments)
            {
                var attachment = new SubmissionAttachment(
                    attachmentCommand.Name,
                    attachmentCommand.Type,
                    attachmentCommand.Url,
                    attachmentCommand.Size
                );
                _attachments.Add(attachment);
            }
        }

        ValidateSubmission();
    }
    
    public void AddLink(string url, string description = "")
    {
        var link = new SubmissionLink(url, description);
        _links.Add(link);
        UpdatedAt = DateTime.Now;
    }

    public void RemoveLink(int linkId)
    {
        var link = _links.FirstOrDefault(l => l.Id == linkId);
        if (link != null)
        {
            _links.Remove(link);
            UpdatedAt = DateTime.Now;
        }
    }

    public void AddAttachment(string name, string type, string url, long size)
    {
        var attachment = new SubmissionAttachment(name, type, url, size);
        _attachments.Add(attachment);
        UpdatedAt = DateTime.Now;
    }

    public void RemoveAttachment(int attachmentId)
    {
        var attachment = _attachments.FirstOrDefault(a => a.Id == attachmentId);
        if (attachment != null)
        {
            _attachments.Remove(attachment);
            UpdatedAt = DateTime.Now;
        }
    }

    public bool CanBeReviewed()
    {
        return Status == SubmissionStatus.SUBMITTED;
    }

    public bool IsCompleted()
    {
        return Status == SubmissionStatus.REVIEWED;
    }

    public bool NeedsRevision()
    {
        return Status == SubmissionStatus.NEEDS_REVISION;
    }

    private void ValidateSubmission()
    {
        if (_links.Count == 0 && _attachments.Count == 0)
        {
            throw new InvalidOperationException("Submission must have at least one link or attachment");
        }

        if (string.IsNullOrWhiteSpace(CollaboratorName))
        {
            throw new InvalidOperationException("Collaborator name is required");
        }
    }
}