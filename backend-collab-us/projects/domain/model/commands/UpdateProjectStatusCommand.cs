namespace backend_collab_us.projects.domain.model.commands;

public record UpdateProjectStatusCommand(int ProjectId, string Status);
