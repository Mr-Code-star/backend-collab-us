using backend_collab_us.projects.domain.model.agregates;
using backend_collab_us.projects.domain.model.commands;
using backend_collab_us.projects.domain.repository;
using backend_collab_us.projects.domain.services;
using backend_collab_us.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace backend_collab_us.projects.Application.Internal.CommandService;

public class FavoriteCommandService(
    IFavoriteRepository favoriteRepository,
    IProjectRepository projectRepository,
    IUnitOfWork unitOfWork
) : IFavoriteCommandService
{
    public async Task<Favorite?> Handle(CreateFavoriteCommand command)
    {
        try
        {
            // Validar que el proyecto existe
            var projectExists = await projectRepository.ExistsByIdAsync(command.ProjectId);
            if (!projectExists)
            {
                Console.WriteLine($"Project with ID {command.ProjectId} does not exist");
                return null;
            }

            // Validar que no es favorito duplicado
            var exists = await favoriteRepository.ExistsByProfileAndProjectAsync(
                command.ProfileId, command.ProjectId);
            if (exists)
            {
                Console.WriteLine($"Project is already in favorites");
                return null;
            }

            // Crear el favorito
            var favorite = new Favorite(command);
            
            // Agregar al repositorio
            await favoriteRepository.AddAsync(favorite);
            
            // Commit changes
            await unitOfWork.CompleteAsync();
            
            return favorite;
        }
        catch (DbUpdateException dbEx)
        {
            Console.WriteLine($"Database error creating favorite: {dbEx.Message}");
            if (dbEx.InnerException != null)
            {
                Console.WriteLine($"Inner exception: {dbEx.InnerException.Message}");
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating favorite: {ex.Message}");
            return null;
        }
    }

   
}