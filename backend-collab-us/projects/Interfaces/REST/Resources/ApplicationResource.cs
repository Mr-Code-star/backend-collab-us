namespace backend_collab_us.projects.Interfaces.REST.Resources;

public record ApplicationResource(
    int Id,
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
    string Status,
    string ReviewNotes,
    int ReviewerId,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    DateTime? ReviewedAt
);