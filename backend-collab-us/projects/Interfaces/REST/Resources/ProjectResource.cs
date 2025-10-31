namespace backend_collab_us.projects.Interfaces.REST.Resources;

public record ProjectResource(
    int Id,
    int UserId,
    string Title,
    string Description,
    string Summary,
    string AcademicLevelName,
    string Benefits,
    List<string> Skills,
    int DurationQuantity,
    string DurationType,
    string Status,
    int Progress,
    List<RoleResource> Roles,
    List<string> Areas,
    List<string> Tags,
    DateTime CreatedAt,
    DateTime UpdatedAt
);