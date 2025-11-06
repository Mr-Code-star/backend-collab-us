namespace backend_collab_us.task_management.domain.model.commands;

public record ProcessOverdueTasksCommand(
    DateTime CurrentDate // Fecha actual para comparación
);