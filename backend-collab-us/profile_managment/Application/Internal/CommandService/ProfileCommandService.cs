using backend_collab_us.profile_managment.domain.model.commands;
using backend_collab_us.profile_managment.domain.model.agregates;
using backend_collab_us.profile_managment.domain.repositories;
using backend_collab_us.profile_managment.domain.services;
using backend_collab_us.Shared.Domain.Repositories;
using backend_collab_us.IAM.domain.repositories; // AGREGAR esta referencia
using Microsoft.EntityFrameworkCore;

namespace backend_collab_us.profile_managment.Application.Internal.CommandService;

public class ProfileCommandService(
    IProfileRepository profileRepository,
    IUserRepository userRepository, // AGREGAR este repositorio
    IUnitOfWork unitOfWork
    ) : IProfileCommandService
{
    /// <summary>
    /// Handles the command to create a new profile in the system (onboarding)
    /// </summary>
    /// <param name="command">Command containing profile data to create</param>
    /// <returns>The created profile or null if there was an error</returns>
    public async Task<Profile?> Handle(CreateProfileCommand command)
    {
        try
        {
            // VALIDAR: Verificar que el UserId existe
            var userExists = await userRepository.ExistsByIdAsync(command.UserId);
            if (!userExists)
            {
                Console.WriteLine($"User with ID {command.UserId} does not exist");
                return null;
            }

            // VALIDAR: Verificar que no existe ya un profile para este UserId
            var existingProfile = await profileRepository.FindByUserIdAsync(command.UserId);
            if (existingProfile != null)
            {
                Console.WriteLine($"Profile already exists for User ID {command.UserId}");
                return null;
            }

            // VALIDAR: Verificar que el username no está tomado
            var existingUsername = await profileRepository.FindByUsernameAsync(command.Username);
            if (existingUsername != null)
            {
                Console.WriteLine($"Username {command.Username} is already taken");
                return null;
            }

            // Create a new Profile instance with command data
            var profile = new Profile(
                command.UserId,
                command.Username,
                command.Avatar,
                command.Role,
                command.Bio,
                command.Abilities,
                command.Experiences,
                command.Cv
            );
                
            // Add profile to the repository
            await profileRepository.AddAsync(profile);
            
            // Commit changes to the database
            await unitOfWork.CompleteAsync();
            
            return profile;
        }
        catch (DbUpdateException dbEx)
        {
            // Log the exception
            Console.WriteLine($"Database error creating profile: {dbEx.Message}");
            
            if (dbEx.InnerException != null)
            {
                Console.WriteLine($"Inner exception: {dbEx.InnerException.Message}");
            }
            
            return null;
        }
        catch (Exception ex)
        {
            // Log other exceptions
            Console.WriteLine($"Error creating profile: {ex.Message}");
            return null;
        }
    }
}