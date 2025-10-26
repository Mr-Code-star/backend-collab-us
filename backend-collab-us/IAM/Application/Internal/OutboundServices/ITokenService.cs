using backend_collab_us.IAM.domain.model.agregates;

namespace backend_collab_us.IAM.Application.Internal.OutboundServices;

/// <summary>
/// Service for generating and validating authentication tokens.
/// Handles JWT token creation and verification for user authentication and authorization.
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Generates a JWT token for a specific user.
    /// </summary>
    /// <param name="user">User entity for which the token is generated</param>
    /// <returns>JWT token as string</returns>
    string GenerateToken(User user);
    
    /// <summary>
    /// Validates a JWT token and extracts the user identifier.
    /// </summary>
    /// <param name="token">JWT token to validate</param>
    /// <returns>User identifier (ID) if the token is valid, null otherwise</returns>
    Task<int?> ValidateToken(string token);
}