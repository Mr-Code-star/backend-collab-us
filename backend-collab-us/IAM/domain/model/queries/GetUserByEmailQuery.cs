namespace backend_collab_us.IAM.domain.model.queries;
/// <summary>
/// Query to obtain one user by email
/// </summary>
/// <param name="Email"></param>
public record GetUserByEmailQuery(string Email);