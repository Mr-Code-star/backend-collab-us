using System.ComponentModel.DataAnnotations;
using backend_collab_us.projects.domain.model.commands;

namespace backend_collab_us.projects.domain.model.valueObjects;

public class RoleCard
{
    public int Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public List<string> Items { get; private set; } = new List<string>();
    public int? RoleId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    protected RoleCard()
    {
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }

    public RoleCard(string title, List<string> items, int? roleId = null)
    {
        Title = title;
        Items = items ?? new List<string>();
        RoleId = roleId;
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }

    // Constructor desde comando
    public RoleCard(CreateRoleCardCommand command, int? roleId = null)
        : this(command.Title, command.Items, roleId)
    {
    }
}