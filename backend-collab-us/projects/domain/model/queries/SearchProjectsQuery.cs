namespace backend_collab_us.projects.domain.model.queries;

public record SearchProjectsQuery(string? Query, string? Area, string? RoleName, string status);
