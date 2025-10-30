namespace backend_collab_us.projects.domain.model.valueObjects;

public class AcademicLevel
{
    public int Id { get; set; }  // Cambia de private set a set
    public string Name { get; set; } = string.Empty; // Agrega valor por defecto
    public string Description { get; set; } = string.Empty; // Agrega valor por defecto
    public int Level { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
}