using System.Threading.Tasks;

namespace API_Project1.Interfaces
{
    public interface IUserInfoService
    {
        //Task<string> GetUserInfoAsync(string accessToken);
        Task<(string UserMa, string DoanhNghiepMa)> GetUserAndCompanyCodeAsync();
        Task<string> GetUserInfoAsync();
    }
}
