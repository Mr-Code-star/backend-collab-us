namespace backend_collab_us.IAM.Application.Internal.OutboundServices;
/// <summary>
/// Service for password hashing and verification. It provides functionalities
/// to create secure password hashes and verify passwords
/// against existing hashes.
/// </summary>
public interface IHashingService
{
    /// <summary>
    /// Create a secure hash from a plain text password. 
    /// </summary>
    /// <param name="password">Plain text password</param>
    /// <returns>
    ///  Hash seguro de la contraseña   
    /// </returns>
    string HashPassword(string password);
    
    /// <summary>
    /// Check if a plain text password matches a stored hash.
    /// </summary>
    /// <param name="password">Plain text password to verify</param>
    /// <param name="passwordHash">Stored hash for comparison</param>
    /// <returns>True if the password matches, False otherwise</returns>
    /// 
    bool VerifyPassword(string password, string passwordHash);
}