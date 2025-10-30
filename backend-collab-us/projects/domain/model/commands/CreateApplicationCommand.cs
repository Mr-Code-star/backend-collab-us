namespace backend_collab_us.projects.domain.model.commands;

/// <summary>
/// Command to create an application for a project
/// </summary>
public record CreateApplicationCommand(
    int ProjectId,
    int ApplicantId,
    string ApplicantName,
    string ApplicantEmail,
    string ApplicantPortfolio,
    string ApplicantPhone,
    long RoleId,
    string Message,
    bool AcceptedTerms,
    string CvFileName,
    string Status = "pending"
);