namespace BL.Services.TokenService
{
    public interface ITokenService
    {
        string GenerateToken(Guid userId);
    }
}
