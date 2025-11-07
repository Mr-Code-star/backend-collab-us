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
    
    public string Username { get; set; }
    public string? Avatar { get; set; }
    public string Role { get; set; }
    public string Bio { get; set; }
    public List<string> Abilities { get; set; }
    public List<Experience> Experiences { get; set; }
    public CV? Cv { get; set; }
    public string Status { get; private set; }
    public int Points { get; set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; set; }

    public List<string> PointsGivenBy { get; set; } = new List<string>();

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
        PointsGivenBy = new List<string>();

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
        PointsGivenBy = new List<string>();

    }

    // Constructor desde comando
    public Profile(CreateProfileCommand command) 
        : this(command.UserId, command.Username, command.Avatar, command.Role, command.Bio, command.Abilities, command.Experiences, command.Cv)
    {
    }
    
    public void AddPoint(string userId)
    {
        if (!PointsGivenBy.Contains(userId))
        {
            PointsGivenBy.Add(userId);
            Points++;
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public void RemovePoint(string userId)
    {
        if (PointsGivenBy.Contains(userId))
        {
            PointsGivenBy.Remove(userId);
            Points = Math.Max(0, Points - 1);
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public bool HasUserGivenPoint(string userId)
    {
        return PointsGivenBy.Contains(userId);
    }

    public void UpdatePoints(int points, List<string> pointsGivenBy)
    {
        Points = Math.Max(0, points);
        PointsGivenBy = pointsGivenBy ?? new List<string>();
        UpdatedAt = DateTime.UtcNow;
    }

    // Método para toggle de puntos
    public bool ToggleUserPoint(string userId)
    {
        if (HasUserGivenPoint(userId))
        {
            RemovePoint(userId);
            return false; // Punto quitado
        }
        else
        {
            AddPoint(userId);
            return true; // Punto agregado
        }
    }
}