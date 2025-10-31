namespace backend_collab_us.projects.Interfaces.REST.Resources;

public record CreateProjectResource(
    int UserId,
    string Title,
    string Description,
    string Summary,
    string? AcademicLevelName,
    string Benefits,
    List<string> Skills,
    int DurationQuantity,
    string DurationType,
    List<string> Areas,
    List<CreateRoleResource> Roles,
    List<string> Tags,
    string Status = "draft",
    int Progress = 0
);