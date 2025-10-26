using backend_collab_us.profile_managment.domain.model.agregates;
using backend_collab_us.profile_managment.domain.repositories;
using backend_collab_us.Shared.Infrastructure.Persistence.EFC.Configuration;
using backend_collab_us.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace backend_collab_us.profile_managment.Infrastructure.Persistence.Repositories;

public class ProfileRepository(AppDbContext context) : BaseRepository<Profile>(context), IProfileRepository
{
    // Implementación específica para ProfileId (string)
    public async Task<Profile?> FindByIdAsync(int profileId)
    {
        return await Context.Set<Profile>()
            .FirstOrDefaultAsync(p => p.Id == profileId);
    }
    
    public new async Task<IEnumerable<Profile>> ListAsync()
    {
        return await Context.Set<Profile>()
            .ToListAsync();
    }
    
    public async Task<Profile?> FindByUsernameAsync(string username)
    {
        return await Context.Set<Profile>()
            .FirstOrDefaultAsync(p => p.Username == username);
    }

    public async Task<IEnumerable<Profile>> SearchProfilesAsync(
        string? query, 
        string? role, 
        int? minScore, 
        int? maxScore)
    {
        var profiles = Context.Set<Profile>().AsQueryable();

        // Aplicar filtros que pueden ser traducidos a SQL
        if (!string.IsNullOrEmpty(role))
        {
            profiles = profiles.Where(p => p.Role.Contains(role));
        }

        if (minScore.HasValue)
        {
            profiles = profiles.Where(p => p.Points >= minScore.Value);
        }

        if (maxScore.HasValue)
        {
            profiles = profiles.Where(p => p.Points <= maxScore.Value);
        }

        // Filtrar solo perfiles activos
        profiles = profiles.Where(p => p.Status == "active");

        // Ejecutar la consulta inicial para obtener datos de la base de datos
        var result = await profiles.ToListAsync();

        // Aplicar filtro de búsqueda en memoria (client-side)
        if (!string.IsNullOrEmpty(query))
        {
            result = result.Where(p => 
                    p.Username.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    p.Abilities.Any(ability => 
                        ability.Contains(query, StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }

        return result;
    }

    public async Task<Profile?> FindByUserIdAsync(int userId)
    {
        return await Context.Set<Profile>()
            .FirstOrDefaultAsync(p => p.UserId == userId);
    }


    public async Task<IEnumerable<Profile>> GetAllActiveProfilesAsync()
    {
        return await Context.Set<Profile>()
            .Where(p => p.Status == "active")
            .ToListAsync();
    }
}