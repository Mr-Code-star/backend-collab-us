using backend_collab_us.profile_managment.domain.model.commands;
using backend_collab_us.profile_managment.domain.model.valueObjects;
using backend_collab_us.IAM.domain.model.agregates; // Agregar esta referencia

namespace backend_collab_us.profile_managment.domain.model.agregates;

public partial class Profile
{
    public int Id { get; private set; } // CAMBIADO de string a int
    public int UserId { get; private set; }
    
    // AGREGAR: Relación con User
    public User User { get; private set; }
    
    public string Username { get; private set; }
    public string? Avatar { get; private set; }
    public string Role { get; private set; }
    public string Bio { get; private set; }
    public List<string> Abilities { get; private set; }
    public List<Experience> Experiences { get; private set; }
    public CV? Cv { get; private set; }
    public string Status { get; private set; }
    public int Points { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    protected Profile()
    {
        Username = string.Empty;
        Avatar = null;
        Role = string.Empty;
        Bio = string.Empty;
        Abilities = new List<string>();
        Experiences = new List<Experience>();
        Cv = null;
        Status = "active";
        Points = 0;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
    
    // Constructor principal - ELIMINAR la generación automática del ID
    public Profile(int userId, string username, string? avatar, string role, string bio, List<string> abilities, List<Experience> experiences, CV? cv)
    {
        UserId = userId;
        Username = username;
        Avatar = avatar;
        Role = role;
        Bio = bio;
        Abilities = abilities ?? new List<string>();
        Experiences = experiences ?? new List<Experience>();
        Cv = cv;
        Status = "active";
        Points = 0;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    // Constructor desde comando
    public Profile(CreateProfileCommand command) 
        : this(command.UserId, command.Username, command.Avatar, command.Role, command.Bio, command.Abilities, command.Experiences, command.Cv)
    {
    }
}