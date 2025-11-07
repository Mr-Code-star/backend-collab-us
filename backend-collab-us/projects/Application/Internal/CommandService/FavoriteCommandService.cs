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
            Console.WriteLine($"🔄 Creating favorite - Profile: {command.ProfileId}, Project: {command.ProjectId}");

            // 1. Validar que el proyecto existe
            var projectExists = await projectRepository.ExistsByIdAsync(command.ProjectId);
            if (!projectExists)
            {
                Console.WriteLine($"❌ Project with ID {command.ProjectId} does not exist");
                return null;
            }

            // 2. 🔥 PRIMERO verificar si ya existe el favorito
            var existingFavorite = await favoriteRepository.FindByProfileAndProjectAsync(
                command.ProfileId, command.ProjectId);
                
            if (existingFavorite != null)
            {
                Console.WriteLine($"⚠️ Favorite already exists - ID: {existingFavorite.Id}");
                return existingFavorite;
            }

            // 3. Crear nuevo favorito solo si no existe
            var favorite = new Favorite(command);
            await favoriteRepository.AddAsync(favorite);
            await unitOfWork.CompleteAsync();
            
            Console.WriteLine($"✅ Favorite created successfully - ID: {favorite.Id}");
            return favorite;
        }
        catch (DbUpdateException dbEx)
        {
            // 🔥 SIMPLIFICADO: Solo log el error y retorna null
            Console.WriteLine($"❌ Database error creating favorite: {dbEx.Message}");
            
            // Intentar recuperar el favorito existente por si acaso
            try 
            {
                var existingFavorite = await favoriteRepository.FindByProfileAndProjectAsync(
                    command.ProfileId, command.ProjectId);
                if (existingFavorite != null)
                {
                    Console.WriteLine($"🔄 Recovered existing favorite after error: {existingFavorite.Id}");
                    return existingFavorite;
                }
            }
            catch (Exception recoveryEx)
            {
                Console.WriteLine($"❌ Error recovering favorite: {recoveryEx.Message}");
            }
            
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error creating favorite: {ex.Message}");
            return null;
        }
    }
}