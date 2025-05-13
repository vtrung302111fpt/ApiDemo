using API_Project1.Services;

namespace API_Project1.Interfaces
{
    public interface ITokenService
    {
        //string AccessToken { get; set; }
        //bool IsTokenExpired();
        //void SetToken(string accessToken, int expiresInSeconds);

        //Task<string> GetAccessTokenAsync();
        Task<string> GetFullLoginResponseAsync();
    }
}
