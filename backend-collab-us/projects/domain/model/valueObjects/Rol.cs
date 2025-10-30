using backend_collab_us.projects.domain.model.commands;

namespace backend_collab_us.projects.domain.model.valueObjects;

public class Rol
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public List<RoleCard> Cards { get; private set; } = new List<RoleCard>();
    public int? ProjectId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    protected Rol()
    {
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }

    public Rol(string name, int? projectId = null)
    {
        Name = name;
        ProjectId = projectId;
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }

    // Constructor desde comando
    public Rol(CreateRoleCommand command, int? projectId = null)
        : this(command.Name, projectId)
    {
        // Agregar cards desde el comando
        if (command.Cards != null)
        {
            foreach (var cardCommand in command.Cards)
            {
                var roleCard = new RoleCard(cardCommand);
                Cards.Add(roleCard);
            }
        }
    }
}