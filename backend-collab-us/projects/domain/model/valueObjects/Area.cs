namespace backend_collab_us.projects.domain.model.valueObjects;

public partial class Area
{
    public int Id { get; private set; } // id automatico definidos seran 10 id osea 10 areas que puede abarcar el projecto
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public bool Active { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
}