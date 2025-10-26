using backend_collab_us.IAM.domain.model.commands;

namespace backend_collab_us.IAM.domain.model.agregates;

public partial class User
{
    public int Id { get; } // ID AUTOMATICO
    public string FullName { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public string Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    protected User()
    {
        FullName = string.Empty;
        Email = string.Empty;
        Status = "active"; // Valor por defecto
        Password = string.Empty;
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }
    
    // Constructor principal
    public User(string fullName, string email, string password)
    {
        FullName = fullName;
        Email = email;
        Status = "active"; // Valor por defecto
        Password = password;
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }

    // Constructor desde comando
    public User(CreateUserCommand command) 
        : this(command.FullName, command.Email, command.Password)
    {
    }
}