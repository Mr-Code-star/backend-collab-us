using backend_collab_us.comment_managment.domain.model.agregates;
using backend_collab_us.IAM.domain.repositories;
using backend_collab_us.profile_managment.domain.model.commands;
using backend_collab_us.profile_managment.domain.repositories;
using backend_collab_us.profile_managment.domain.services;
using backend_collab_us.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace backend_collab_us.profile_managment.Application.Internal.CommandService;

public class CommentCommandService(
    ICommentRepository commentRepository,
    IProfileRepository profileRepository,
    IUserRepository  userRepository,
    IUnitOfWork unitOfWork
) : ICommentCommandService
{
    /// <summary>
    /// Handles the command to create a new comment
    /// </summary>
    /// <param name="command">Command containing comment data to create</param>
    /// <returns>The created comment or null if there was an error</returns>
    public async Task<Comment?> Handle(CreateCommentCommand command)
    {
        try
        {
            // VALIDAR: Verificar que el ProfileId existe
            var profileExists = await profileRepository.FindByIdAsync(command.ProfileId);
            if (profileExists == null)
            {
                Console.WriteLine($"Profile with ID {command.ProfileId} does not exist");
                return null;
            }
            
            // AGREGAR: Validar que el UserId existe
            var userExists = await userRepository.FindByIdAsync(command.UserId);
            if (userExists == null)
            {
                Console.WriteLine($"User with ID {command.UserId} does not exist");
                return null;
            }
            
            // Crear una nueva instancia de Comment con los datos del comando
            var comment = new Comment(
                command.ProfileId,
                command.UserId,
                command.Rating,
                command.Comment
            );
                
            // Agregar comentario al repositorio
            await commentRepository.AddAsync(comment);
            
            // Commit changes to the database
            await unitOfWork.CompleteAsync();
            
            return comment;
        }
        catch (DbUpdateException dbEx)
        {
            // Log the exception
            Console.WriteLine($"Database error creating comment: {dbEx.Message}");
            
            if (dbEx.InnerException != null)
            {
                Console.WriteLine($"Inner exception: {dbEx.InnerException.Message}");
            }
            
            return null;
        }
        catch (Exception ex)
        {
            // Log other exceptions
            Console.WriteLine($"Error creating comment: {ex.Message}");
            return null;
        }
    }
}