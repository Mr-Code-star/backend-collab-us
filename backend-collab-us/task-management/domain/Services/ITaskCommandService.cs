using backend_collab_us.task_management.domain.model.commands;
using Task = backend_collab_us.task_management.domain.model.agregates.Task;
namespace backend_collab_us.task_management.domain.Services;

public interface ITaskCommandService
{
    Task<Task?> Handle(CreateTaskCommand command);
    Task<Task?> Handle(UpdateTaskDueDateCommand command);
    Task<Task?> Handle(UpdateTaskStatusCommand command);
    Task<Task?> Handle(UpdateTaskCommand command);
    Task<bool> Handle(DeleteTaskCommand command);
    Task<Task?> Handle(ProcessOverdueTasksCommand command);
    // Metodos para gestion de checklist
    Task<Task?> Handle(AddChecklistItemCommand command);
    Task<Task?> Handle(RemoveChecklistItemCommand command);
    Task<Task?> Handle(ToggleChecklistItemCommand command);
    // Métodos para gestión de herramientas
    Task<Task?> Handle(AddTaskToolCommand command);
    Task<Task?> Handle(RemoveTaskToolCommand command);
    Task<Task?> Handle(ToggleTaskToolCommand command);
    // Métodos para adjuntos
    Task<Task?> Handle(AddTaskAttachmentCommand command);

    Task<IEnumerable<Task>> CreateTasksForAllAcceptedApplicants(CreateTaskCommand baseCommand);
}