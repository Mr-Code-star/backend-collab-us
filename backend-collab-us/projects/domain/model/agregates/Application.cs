using backend_collab_us.IAM.domain.model.agregates;
using backend_collab_us.projects.domain.model.commands;

namespace backend_collab_us.projects.domain.model.agregates;

public partial class Application
{
    public int Id { get; private set; }
    public int ProjectId { get; private set; }
    public int ApplicantId { get; private set; }
    public string ApplicantName { get; private set; } = string.Empty;
    public string ApplicantEmail { get; private set; } = string.Empty;
    public string ApplicantPortfolio { get; private set; } = string.Empty;
    public string ApplicantPhone { get; private set; } = string.Empty;
    public long RoleId { get; private set; }
    public string Message { get; private set; } = string.Empty;
    public bool AcceptedTerms { get; private set; }
    public string CvFileName { get; private set; } = string.Empty;
    public string Status { get; private set; } = "pending";
    public string ReviewNotes { get; private set; } = string.Empty;
    public int ReviewerId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime? ReviewedAt { get; private set; }

    // Relaciones
    public Project Project { get; private set; }
    public User Applicant { get; private set; } // Relación con el usuario aplicante
    
    
    public Application(
        int projectId,
        int applicantId,
        string applicantName,
        string applicantEmail,
        long roleId,
        string message,
        bool acceptedTerms,
        string cvFileName = "",
        string applicantPortfolio = "",
        string applicantPhone = "",
        string status = "pending")
    {
        ProjectId = projectId;
        ApplicantId = applicantId;
        ApplicantName = applicantName;
        ApplicantEmail = applicantEmail;
        ApplicantPortfolio = applicantPortfolio;
        ApplicantPhone = applicantPhone;
        RoleId = roleId;
        Message = message;
        AcceptedTerms = acceptedTerms;
        CvFileName = cvFileName;
        Status = status;
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }
    
    public Application(CreateApplicationCommand command)
    {
        ProjectId = command.ProjectId;
        ApplicantId = command.ApplicantId;
        ApplicantName = command.ApplicantName;
        ApplicantEmail = command.ApplicantEmail;
        ApplicantPortfolio = command.ApplicantPortfolio ?? string.Empty;
        ApplicantPhone = command.ApplicantPhone ?? string.Empty;
        RoleId = command.RoleId;
        Message = command.Message;
        AcceptedTerms = command.AcceptedTerms;
        CvFileName = command.CvFileName ?? string.Empty;
        Status = command.Status;
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }
    
    public void UpdateStatus(string status, string reviewNotes = "", int reviewerId = 0)
    {
        Status = status;
    
        if (!string.IsNullOrEmpty(reviewNotes))
        {
            ReviewNotes = reviewNotes;
        }
    
        if (reviewerId > 0)
        {
            ReviewerId = reviewerId;
            ReviewedAt = DateTime.Now;
        }
    
        UpdatedAt = DateTime.Now;
    }
    
    public void UpdateStatus(UpdateApplicationStatusCommand command)
    {
        UpdateStatus(command.Status, command.ReviewNotes, command.ReviewerId);
    }
}