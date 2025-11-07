using Task = backend_collab_us.task_management.domain.model.agregates.Task;
using TaskStatus = backend_collab_us.task_management.domain.model.valueObjects.TaskStatus;
using backend_collab_us.projects.domain.repositories;
using backend_collab_us.projects.domain.repository;
using backend_collab_us.Shared.Domain.Repositories;
using backend_collab_us.task_management.domain.model.commands;
using backend_collab_us.task_management.domain.repositories;
using backend_collab_us.task_management.domain.Services;

namespace backend_collab_us.task_management.Application.Internal.CommandService;

public class TaskCommandService : ITaskCommandService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IApplicationRepository _applicationRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TaskCommandService(
        ITaskRepository taskRepository,
        IApplicationRepository applicationRepository,
        IProjectRepository projectRepository,
        IUnitOfWork unitOfWork)
    {
        _taskRepository = taskRepository;
        _applicationRepository = applicationRepository;
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Task?> Handle(CreateTaskCommand command)
    {
        try
        {
            // Validar que el proyecto existe
            var projectExists = await _projectRepository.ExistsByIdAsync(command.ProjectId);
            if (!projectExists)
            {
                throw new ArgumentException($"Project with ID {command.ProjectId} does not exist");
            }

            // Validar que el aplicante existe y tiene status "accepted"
            var application = await ValidateApplicantIsAccepted(command.ProjectId, command.AssignedTo);
            
            // Crear la tarea
            var task = new Task(command);
            
            // Agregar al repositorio
            await _taskRepository.AddAsync(task);
            await _unitOfWork.CompleteAsync();
            
            return task;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating task: {ex.Message}");
            throw;
        }
    }

    private async Task<backend_collab_us.projects.domain.model.agregates.Application> ValidateApplicantIsAccepted(int projectId, int applicantId)
    {
        // Obtener todas las aplicaciones del proyecto
        var applications = await _applicationRepository.GetByProjectIdAsync(projectId);
        
        // Buscar la aplicación específica del aplicante con status "accepted"
        var acceptedApplication = applications.FirstOrDefault(app => 
            app.ApplicantId == applicantId && 
            app.Status.ToLower() == "accepted");

        if (acceptedApplication == null)
        {
            throw new ArgumentException($"Applicant with ID {applicantId} is not accepted in project {projectId} or does not exist");
        }

        return acceptedApplication;
    }

    // Método para crear múltiples tareas para todos los aplicantes aceptados
    public async Task<IEnumerable<Task>> CreateTasksForAllAcceptedApplicants(CreateTaskCommand baseCommand)
    {
        try
        {
            // Obtener aplicaciones aceptadas para el proyecto
            var applications = await _applicationRepository.GetByProjectIdAsync(baseCommand.ProjectId);
            var acceptedApplications = applications.Where(app => app.Status.ToLower() == "accepted").ToList();

            if (!acceptedApplications.Any())
            {
                throw new ArgumentException($"No accepted applicants found for project ID {baseCommand.ProjectId}");
            }

            var createdTasks = new List<Task>();

            foreach (var application in acceptedApplications)
            {
                // Crear un comando de tarea para cada aplicante aceptado
                var taskCommand = new CreateTaskCommand(
                    Title: $"{baseCommand.Title} - {application.ApplicantName}",
                    Description: baseCommand.Description,
                    DueDate: baseCommand.DueDate,
                    Status: baseCommand.Status,
                    Priority: baseCommand.Priority,
                    ProjectId: baseCommand.ProjectId,
                    AssignedTo: application.ApplicantId,
                    AssignedToName: application.ApplicantName,
                    Role: application.RoleId.ToString(),
                    Checklist: baseCommand.Checklist,
                    Tools: baseCommand.Tools,
                    Comment: baseCommand.Comment,
                    Attachments: baseCommand.Attachments,
                    EstimatedHours: baseCommand.EstimatedHours,
                    CreatedBy: baseCommand.CreatedBy
                );

                // Crear la tarea individual usando el método principal
                var task = await Handle(taskCommand);
                if (task != null)
                {
                    createdTasks.Add(task);
                }
            }

            return createdTasks;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating tasks for all accepted applicants: {ex.Message}");
            throw;
        }
    }

    public async Task<Task?> Handle(UpdateTaskDueDateCommand command)
    {
        try
        {
            var task = await _taskRepository.FindByIdWithRelationsAsync(command.TaskId);
            if (task == null)
            {
                throw new ArgumentException($"Task with ID {command.TaskId} not found");
            }

            if (task.ProjectId != command.ProjectId)
            {
                throw new ArgumentException("Task does not belong to the specified project");
            }

            task.UpdateDueDate(command);
            _taskRepository.Update(task);
            await _unitOfWork.CompleteAsync();
            
            return task;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating task due date: {ex.Message}");
            throw;
        }
    }

    public async Task<Task?> Handle(UpdateTaskStatusCommand command)
    {
        try
        {
            var task = await _taskRepository.FindByIdWithRelationsAsync(command.TaskId);
            if (task == null)
            {
                throw new ArgumentException($"Task with ID {command.TaskId} not found");
            }

            if (task.ProjectId != command.ProjectId)
            {
                throw new ArgumentException("Task does not belong to the specified project");
            }

            task.Status = command.Status;
            
            if (command.Status == TaskStatus.COMPLETED)
            {
                task.MarkAsCompleted();
            }

            task.UpdatedAt = DateTime.Now;
            
            _taskRepository.Update(task);
            await _unitOfWork.CompleteAsync();
            
            return task;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating task status: {ex.Message}");
            throw;
        }
    }

    public async Task<Task?> Handle(UpdateTaskCommand command)
    {
        try
        {
            var task = await _taskRepository.FindByIdWithRelationsAsync(command.TaskId);
            if (task == null)
            {
                throw new ArgumentException($"Task with ID {command.TaskId} not found");
            }

            if (task.ProjectId != command.ProjectId)
            {
                throw new ArgumentException("Task does not belong to the specified project");
            }

            // Validar que el nuevo asignado (si cambió) también sea un aplicante aceptado
            if (command.AssignedTo != task.AssignedTo)
            {
                await ValidateApplicantIsAccepted(command.ProjectId, command.AssignedTo);
            }

            task.Title = command.Title;
            task.Description = command.Description;
            task.DueDate = command.DueDate;
            task.Status = command.Status;
            task.Priority = command.Priority;
            task.AssignedTo = command.AssignedTo;
            task.AssignedToName = command.AssignedToName;
            task.Role = command.Role;
            task.Comment = command.Comment;
            task.EstimatedHours = command.EstimatedHours;
            task.ActualHours = command.ActualHours;
            task.UpdatedAt = DateTime.Now;

            task.UpdateStatusBasedOnDueDate();

            _taskRepository.Update(task);
            await _unitOfWork.CompleteAsync();
            
            return task;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating task: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> Handle(DeleteTaskCommand command)
    {
        try
        {
            var task = await _taskRepository.FindByIdAsync(command.TaskId);
            if (task == null)
            {
                throw new ArgumentException($"Task with ID {command.TaskId} not found");
            }

            if (task.ProjectId != command.ProjectId)
            {
                throw new ArgumentException("Task does not belong to the specified project");
            }

            _taskRepository.Remove(task);
            await _unitOfWork.CompleteAsync();
            
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting task: {ex.Message}");
            throw;
        }
    }

    public async Task<Task?> Handle(ProcessOverdueTasksCommand command)
    {
        try
        {
            var tasks = await _taskRepository.ListAsync();
            
            foreach (var task in tasks)
            {
                task.ProcessOverdueStatus(command);
                _taskRepository.Update(task);
            }

            await _unitOfWork.CompleteAsync();
            
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing overdue tasks: {ex.Message}");
            throw;
        }
    }

    public async Task<Task?> Handle(AddChecklistItemCommand command)
    {
        try
        {
            var task = await _taskRepository.FindByIdWithRelationsAsync(command.TaskId);
            if (task == null) throw new ArgumentException($"Task with ID {command.TaskId} not found");
            if (task.ProjectId != command.ProjectId) throw new ArgumentException("Task does not belong to the specified project");

            // task.AddChecklistItem(command.Text);
            _taskRepository.Update(task);
            await _unitOfWork.CompleteAsync();
            return task;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding checklist item: {ex.Message}");
            throw;
        }
    }

    public async Task<Task?> Handle(RemoveChecklistItemCommand command)
    {
        try
        {
            var task = await _taskRepository.FindByIdWithRelationsAsync(command.TaskId);
            if (task == null) throw new ArgumentException($"Task with ID {command.TaskId} not found");
            if (task.ProjectId != command.ProjectId) throw new ArgumentException("Task does not belong to the specified project");

            // task.RemoveChecklistItem(command.ChecklistItemId);
            _taskRepository.Update(task);
            await _unitOfWork.CompleteAsync();
            return task;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error removing checklist item: {ex.Message}");
            throw;
        }
    }

    public async Task<Task?> Handle(ToggleChecklistItemCommand command)
    {
        try
        {
            var task = await _taskRepository.FindByIdWithRelationsAsync(command.TaskId);
            if (task == null) throw new ArgumentException($"Task with ID {command.TaskId} not found");
            if (task.ProjectId != command.ProjectId) throw new ArgumentException("Task does not belong to the specified project");

            // task.ToggleChecklistItem(command.ChecklistItemId);
            task.UpdateProgress();
            _taskRepository.Update(task);
            await _unitOfWork.CompleteAsync();
            return task;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error toggling checklist item: {ex.Message}");
            throw;
        }
    }

    public async Task<Task?> Handle(AddTaskToolCommand command)
    {
        try
        {
            var task = await _taskRepository.FindByIdWithRelationsAsync(command.TaskId);
            if (task == null) throw new ArgumentException($"Task with ID {command.TaskId} not found");
            if (task.ProjectId != command.ProjectId) throw new ArgumentException("Task does not belong to the specified project");

            // task.AddTool(command.Name);
            _taskRepository.Update(task);
            await _unitOfWork.CompleteAsync();
            return task;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding task tool: {ex.Message}");
            throw;
        }
    }

    public async Task<Task?> Handle(RemoveTaskToolCommand command)
    {
        try
        {
            var task = await _taskRepository.FindByIdWithRelationsAsync(command.TaskId);
            if (task == null) throw new ArgumentException($"Task with ID {command.TaskId} not found");
            if (task.ProjectId != command.ProjectId) throw new ArgumentException("Task does not belong to the specified project");

            // task.RemoveTool(command.ToolId);
            _taskRepository.Update(task);
            await _unitOfWork.CompleteAsync();
            return task;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error removing task tool: {ex.Message}");
            throw;
        }
    }

    public async Task<Task?> Handle(ToggleTaskToolCommand command)
    {
        try
        {
            var task = await _taskRepository.FindByIdWithRelationsAsync(command.TaskId);
            if (task == null) throw new ArgumentException($"Task with ID {command.TaskId} not found");
            if (task.ProjectId != command.ProjectId) throw new ArgumentException("Task does not belong to the specified project");

            // task.ToggleTool(command.ToolId);
            _taskRepository.Update(task);
            await _unitOfWork.CompleteAsync();
            return task;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error toggling task tool: {ex.Message}");
            throw;
        }
    }

    public async Task<Task?> Handle(AddTaskAttachmentCommand command)
    {
        try
        {
            var task = await _taskRepository.FindByIdWithRelationsAsync(command.TaskId);
            if (task == null) throw new ArgumentException($"Task with ID {command.TaskId} not found");
            if (task.ProjectId != command.ProjectId) throw new ArgumentException("Task does not belong to the specified project");

            // task.AddAttachment(command.Name, command.Type, command.Url, command.Icon);
            _taskRepository.Update(task);
            await _unitOfWork.CompleteAsync();
            return task;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding task attachment: {ex.Message}");
            throw;
        }
    }
}