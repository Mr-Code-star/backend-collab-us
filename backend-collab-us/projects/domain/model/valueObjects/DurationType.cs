namespace backend_collab_us.projects.domain.model.valueObjects;

public class DurationType
{
    public int Id { get; set; }  // Cambia de private set a set
    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public int Multiplier { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

}