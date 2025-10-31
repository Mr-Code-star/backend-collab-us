using backend_collab_us.projects.domain.model.agregates;
using backend_collab_us.projects.domain.model.commands;

namespace backend_collab_us.projects.domain.services;

public interface IApplicationCommandService
{
    Task<model.agregates.Application?> Handle(CreateApplicationCommand command);
    Task<model.agregates.Application?> Handle(UpdateApplicationStatusCommand command);
    Task<model.agregates.Application?> Handle(AcceptApplicationCommand command);
    Task<model.agregates.Application?> Handle(RejectApplicationCommand command);
}