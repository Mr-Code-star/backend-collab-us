using backend_collab_us.projects.domain.model.commands;

namespace backend_collab_us.projects.domain.model.agregates;

public partial class Favorite
{
    public int Id { get; private set; }
    public int ProfileId { get; private set; }
    public int ProjectId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    public Favorite(int profileId, int projectId)
    {
        ProfileId = profileId;
        ProjectId = projectId;
        CreatedAt = DateTime.Now;
    }
    
    public Favorite(CreateFavoriteCommand command)
    {
        ProfileId = command.ProfileId;
        ProjectId = command.ProjectId;
        CreatedAt = DateTime.Now;
    }
}