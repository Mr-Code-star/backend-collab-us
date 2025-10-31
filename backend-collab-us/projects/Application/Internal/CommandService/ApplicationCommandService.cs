using backend_collab_us.IAM.domain.repositories;
using backend_collab_us.projects.domain.model.commands;
using backend_collab_us.projects.domain.repositories;
using backend_collab_us.projects.domain.repository;
using backend_collab_us.projects.domain.services;
using backend_collab_us.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace backend_collab_us.projects.Application.Internal.CommandService;

public class ApplicationCommandService(
    IApplicationRepository applicationRepository,
    IProjectRepository projectRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork
) : IApplicationCommandService
{
    public async Task<domain.model.agregates.Application?> Handle(CreateApplicationCommand command)
    {
        try
        {
            // Validar que el proyecto existe
            var project = await projectRepository.FindByIdAsync(command.ProjectId);
            if (project == null)
            {
                Console.WriteLine($"Project with ID {command.ProjectId} does not exist");
                return null;
            }

            // Validar que el aplicante existe
            var applicantExists = await userRepository.ExistsByIdAsync(command.ApplicantId);
            if (!applicantExists)
            {
                Console.WriteLine($"Applicant with ID {command.ApplicantId} does not exist");
                return null;
            }

            // Validar que no existe una aplicación duplicada
            var existsDuplicate = await applicationRepository.ExistsByProjectAndApplicantAsync(
                command.ProjectId, command.ApplicantId);
            if (existsDuplicate)
            {
                Console.WriteLine($"Application already exists for this project and applicant");
                return null;
            }

            // Validar términos aceptados
            if (!command.AcceptedTerms)
            {
                Console.WriteLine($"Terms must be accepted to apply");
                return null;
            }

            // Crear la aplicación
            var application = new domain.model.agregates.Application(command);
            
            // Agregar al repositorio
            await applicationRepository.AddAsync(application);
            
            // Commit changes
            await unitOfWork.CompleteAsync();
            
            return application;
        }
        catch (DbUpdateException dbEx)
        {
            Console.WriteLine($"Database error creating application: {dbEx.Message}");
            if (dbEx.InnerException != null)
            {
                Console.WriteLine($"Inner exception: {dbEx.InnerException.Message}");
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating application: {ex.Message}");
            return null;
        }
    }

    public async Task<domain.model.agregates.Application?> Handle(UpdateApplicationStatusCommand command)
    {
        try
        {
            var application = await applicationRepository.FindByIdAsync(command.ApplicationId);
            if (application == null)
            {
                Console.WriteLine($"Application with ID {command.ApplicationId} not found");
                return null;
            }

            application.UpdateStatus(command);
            
            applicationRepository.Update(application);
            await unitOfWork.CompleteAsync();
            
            return application;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating application status: {ex.Message}");
            return null;
        }
    }

    public async Task<domain.model.agregates.Application?> Handle(AcceptApplicationCommand command)
    {
        var updateCommand = new UpdateApplicationStatusCommand(
            command.ApplicationId,
            "accepted",
            command.ReviewNotes,
            command.ReviewerId
        );
        
        return await Handle(updateCommand);
    }

    public async Task<domain.model.agregates.Application?> Handle(RejectApplicationCommand command)
    {
        var updateCommand = new UpdateApplicationStatusCommand(
            command.ApplicationId,
            "rejected", 
            command.ReviewNotes,
            command.ReviewerId
        );
        
        return await Handle(updateCommand);
    }
}