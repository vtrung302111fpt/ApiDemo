using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;


namespace API_Project1.Interfaces
{
	public interface ILoginService
	{
		Task<string> GetFullLoginResponse();
		Task<string> GetAccessTokenAsync();
	}
}