using backend_collab_us.projects.domain.model.valueObjects;

namespace backend_collab_us.projects.domain.model.commands;

/// <summary>
/// Command to create a project
/// </summary>
public record CreateProjectCommand(
    int UserId,
    string Title,
    string Description,
    string Summary,
    AcademicLevel AcademicLevelName,
    string Benefits,
    List<string> Skills,
    int DurationQuantity,
    DurationType DurationType,
    List<string> Areas,
    List<CreateRoleCommand> Roles,
    List<string> Tags, 
    string Status = "draft",
    int Progress = 0
);
