using backend_collab_us.IAM.domain.repositories;
using backend_collab_us.projects.domain.model.agregates;
using backend_collab_us.projects.domain.model.commands;
using backend_collab_us.projects.domain.repository;
using backend_collab_us.projects.domain.services;
using backend_collab_us.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace backend_collab_us.projects.Application.Internal.CommandService;

public class ProjectCommandService(
    IProjectRepository projectRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork
) : IProjectCommandService
{
    public async Task<Project?> Handle(CreateProjectCommand command)
    {
        try
        {
            // Validar que el usuario existe
            var userExists = await userRepository.ExistsByIdAsync(command.UserId);
            if (!userExists)
            {
                Console.WriteLine($"User with ID {command.UserId} does not exist");
                return null;
            }

            // Validar que no existe un proyecto con el mismo título para este usuario
            var existsByTitle = await projectRepository.ExistsByTitleAndUserIdAsync(command.Title, command.UserId);
            if (existsByTitle)
            {
                Console.WriteLine($"Project with title '{command.Title}' already exists for this user");
                return null;
            }

            // Crear el proyecto
            var project = new Project(command);
            
            // Agregar al repositorio
            await projectRepository.AddAsync(project);
            
            // Commit changes
            await unitOfWork.CompleteAsync();
            
            return project;
        }
        catch (DbUpdateException dbEx)
        {
            Console.WriteLine($"Database error creating project: {dbEx.Message}");
            if (dbEx.InnerException != null)
            {
                Console.WriteLine($"Inner exception: {dbEx.InnerException.Message}");
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating project: {ex.Message}");
            return null;
        }
    }

    public async Task<Project?> Handle(UpdateProjectStatusCommand command)
    {
        try
        {
            var project = await projectRepository.FindByIdAsync(command.ProjectId);
            if (project == null)
            {
                Console.WriteLine($"Project with ID {command.ProjectId} not found");
                return null;
            }

            // Aquí necesitarías un método en la entidad Project para actualizar el estado
            // project.UpdateStatus(command.Status);
            
            projectRepository.Update(project);
            await unitOfWork.CompleteAsync();
            
            return project;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating project status: {ex.Message}");
            return null;
        }
    }

}