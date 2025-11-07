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
    
    public async Task<Profile?> Handle(UpdateProfilePointsCommand command)
    {
        try
        {
            Console.WriteLine($"🔄 Actualizando puntos para perfil {command.ProfileId}");
            Console.WriteLine($"📊 Nuevos puntos: {command.Points}");
            Console.WriteLine($"👥 Usuarios que dieron puntos: {string.Join(", ", command.PointsGivenBy)}");

            // Buscar el perfil
            var profile = await profileRepository.FindByIdAsync(command.ProfileId);
            if (profile == null)
            {
                Console.WriteLine($"❌ Perfil con ID {command.ProfileId} no encontrado");
                return null;
            }

            Console.WriteLine($"📋 Puntos anteriores: {profile.Points}");
            Console.WriteLine($"📋 Lista anterior: {string.Join(", ", profile.PointsGivenBy)}");

            // Actualizar puntos y lista de usuarios que dieron puntos
            profile.UpdatePoints(command.Points, command.PointsGivenBy);

            Console.WriteLine($"✅ Puntos actualizados exitosamente");

            // Actualizar en el repositorio
            profileRepository.Update(profile);

            // Commit changes
            await unitOfWork.CompleteAsync();

            return profile;
        }
        catch (DbUpdateException dbEx)
        {
            Console.WriteLine($"❌ Error de base de datos: {dbEx.Message}");
            if (dbEx.InnerException != null)
            {
                Console.WriteLine($"❌ Inner exception: {dbEx.InnerException.Message}");
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error general: {ex.Message}");
            return null;
        }
    }

    public async  Task<Profile?> Handle(UpdateProfileCommand command)
    {
        try
        {
            // Buscar el perfil
            var profile = await profileRepository.FindByIdAsync(command.ProfileId);
            if (profile == null)
            {
                Console.WriteLine($"Profile with ID {command.ProfileId} not found");
                return null;
            }

            // Actualizar campos si se proporcionan
            if (!string.IsNullOrEmpty(command.Username) && command.Username != profile.Username)
                profile.Username = command.Username;
        
            if (command.Avatar != null)
                profile.Avatar = command.Avatar;
        
            if (!string.IsNullOrEmpty(command.Role))
                profile.Role = command.Role;
        
            if (!string.IsNullOrEmpty(command.Bio))
                profile.Bio = command.Bio;
        
            if (command.Abilities != null)
                profile.Abilities = command.Abilities;
        
            if (command.Experiences != null)
                profile.Experiences = command.Experiences;
        
            if (command.Cv != null)
                profile.Cv = command.Cv;
        
            // Actualizar puntos si se proporcionan
            if (command.Points.HasValue)
                profile.Points = command.Points.Value;
        
            if (command.PointsGivenBy != null)
                profile.PointsGivenBy = command.PointsGivenBy;

            profile.UpdatedAt = DateTime.UtcNow;

            // Actualizar en el repositorio
            profileRepository.Update(profile);
        
            // Commit changes
            await unitOfWork.CompleteAsync();
        
            return profile;
        }
        catch (DbUpdateException dbEx)
        {
            Console.WriteLine($"Database error updating profile: {dbEx.Message}");
            if (dbEx.InnerException != null)
            {
                Console.WriteLine($"Inner exception: {dbEx.InnerException.Message}");
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating profile: {ex.Message}");
            return null;
        }
        
    }
}