using backend_collab_us.Shared.Domain.Repositories;
using backend_collab_us.task_management.domain.model.agregates;
using backend_collab_us.task_management.domain.model.commands;
using backend_collab_us.task_management.domain.model.valueObjects;
using backend_collab_us.task_management.domain.repositories;
using backend_collab_us.task_management.domain.Services;

namespace backend_collab_us.task_management.Application.Internal.CommandService;

public class TaskSubmissionCommandService : ITaskSubmissionCommandService
{
    private readonly ITaskSubmissionRepository _taskSubmissionRepository;
    private readonly ITaskRepository _taskRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TaskSubmissionCommandService(
        ITaskSubmissionRepository taskSubmissionRepository,
        ITaskRepository taskRepository,
        IUnitOfWork unitOfWork)
    {
        _taskSubmissionRepository = taskSubmissionRepository;
        _taskRepository = taskRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TaskSubmission?> Handle(CreateTaskSubmissionCommand command)
    {
        try
        {
            // Validar que la tarea existe
            var task = await _taskRepository.FindByIdAsync(command.TaskId);
            if (task == null)
            {
                throw new ArgumentException($"Task with ID {command.TaskId} does not exist");
            }

            // Validar que no existe ya una submission para esta tarea y colaborador
            var existingSubmission = await _taskSubmissionRepository.GetByTaskAndCollaboratorAsync(command.TaskId, command.CollaboratorId);
            if (existingSubmission != null)
            {
                throw new InvalidOperationException($"A submission already exists for task {command.TaskId} and collaborator {command.CollaboratorId}");
            }

            // Validar que el colaborador está asignado a la tarea
            if (task.AssignedTo != command.CollaboratorId)
            {
                throw new InvalidOperationException($"Collaborator {command.CollaboratorId} is not assigned to task {command.TaskId}");
            }

            // Crear la submission
            var submission = new TaskSubmission(command);
            
            // Agregar al repositorio
            await _taskSubmissionRepository.AddAsync(submission);
            await _unitOfWork.CompleteAsync();
            
            return submission;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating task submission: {ex.Message}");
            throw;
        }
    }

    public async Task<TaskSubmission?> Handle(UpdateTaskSubmissionCommand command)
    {
        try
        {
            var submission = await _taskSubmissionRepository.FindByIdAsync(command.SubmissionId);
            if (submission == null)
            {
                throw new ArgumentException($"Task submission with ID {command.SubmissionId} not found");
            }

            // Solo se puede actualizar si está en estado submitted o needs_revision
            if (submission.Status != SubmissionStatus.SUBMITTED && submission.Status != SubmissionStatus.NEEDS_REVISION)
            {
                throw new InvalidOperationException($"Cannot update submission in {submission.Status} status");
            }

            // Actualizar notas si se proporcionan
            if (!string.IsNullOrEmpty(command.Notes))
            {
                submission.Notes = command.Notes;
            }

            // Agregar nuevos links
            if (command.Links != null)
            {
                foreach (var linkCommand in command.Links)
                {
                    submission.AddLink(linkCommand.Url, linkCommand.Description);
                }
            }

            // Agregar nuevos attachments
            if (command.Attachments != null)
            {
                foreach (var attachmentCommand in command.Attachments)
                {
                    submission.AddAttachment(
                        attachmentCommand.Name,
                        attachmentCommand.Type,
                        attachmentCommand.Url,
                        attachmentCommand.Size
                    );
                }
            }

            _taskSubmissionRepository.Update(submission);
            await _unitOfWork.CompleteAsync();
            
            return submission;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating task submission: {ex.Message}");
            throw;
        }
    }

    public async Task<TaskSubmission?> Handle(ReviewTaskSubmissionCommand command)
    {
        try
        {
            var submission = await _taskSubmissionRepository.FindByIdAsync(command.SubmissionId);
            if (submission == null)
            {
                throw new ArgumentException($"Task submission with ID {command.SubmissionId} not found");
            }

            // Marcar como revisado
            submission.MarkAsReviewed(command.ReviewerId, command.ReviewNotes);

            _taskSubmissionRepository.Update(submission);
            await _unitOfWork.CompleteAsync();
            
            return submission;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reviewing task submission: {ex.Message}");
            throw;
        }
    }

    public async Task<TaskSubmission?> Handle(RequestSubmissionRevisionCommand command)
    {
        try
        {
            var submission = await _taskSubmissionRepository.FindByIdAsync(command.SubmissionId);
            if (submission == null)
            {
                throw new ArgumentException($"Task submission with ID {command.SubmissionId} not found");
            }

            // Solicitar revisión
            submission.RequestRevision(command.RevisionNotes);

            _taskSubmissionRepository.Update(submission);
            await _unitOfWork.CompleteAsync();
            
            return submission;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error requesting submission revision: {ex.Message}");
            throw;
        }
    }

    public async Task<TaskSubmission?> Handle(ResubmitTaskSubmissionCommand command)
    {
        try
        {
            var submission = await _taskSubmissionRepository.FindByIdAsync(command.SubmissionId);
            if (submission == null)
            {
                throw new ArgumentException($"Task submission with ID {command.SubmissionId} not found");
            }

            // Resubmitir
            submission.Resubmit(command.NewLinks, command.NewAttachments, command.Notes);

            _taskSubmissionRepository.Update(submission);
            await _unitOfWork.CompleteAsync();
            
            return submission;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error resubmitting task submission: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> Handle(DeleteTaskSubmissionCommand command)
    {
        try
        {
            var submission = await _taskSubmissionRepository.FindByIdAsync(command.SubmissionId);
            if (submission == null)
            {
                throw new ArgumentException($"Task submission with ID {command.SubmissionId} not found");
            }

            _taskSubmissionRepository.Remove(submission);
            await _unitOfWork.CompleteAsync();
            
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting task submission: {ex.Message}");
            throw;
        }
    }
}