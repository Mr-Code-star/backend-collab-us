using backend_collab_us.profile_managment.domain.model.commands;
using backend_collab_us.profile_managment.domain.model.agregates;

namespace backend_collab_us.profile_managment.domain.services;

public interface IProfileCommandService
{
    // Crear solo el profile
    Task<Profile?> Handle(CreateProfileCommand command);
    Task<Profile?> Handle(UpdateProfilePointsCommand command); // NUEVO

    Task<Profile?> Handle(UpdateProfileCommand command);

}