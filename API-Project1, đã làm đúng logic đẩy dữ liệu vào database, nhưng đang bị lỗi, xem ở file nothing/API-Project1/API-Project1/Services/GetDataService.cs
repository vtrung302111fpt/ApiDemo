using API_Project1.Interfaces;

namespace API_Project1.Services
{
    public class GetDataService : IGetDataService
    {

        private readonly ITokenService _tokenService;
        private readonly IUserInfoService _userInfoService;
        private readonly IInvoiceListService _invoiceListService;
        private readonly IInvoiceDetailService _invoiceDetailService;


        public GetDataService(ITokenService tokenService, IUserInfoService userInfoService, IInvoiceListService invoiceListService, IInvoiceDetailService invoiceDetailService)
        {
            _tokenService = tokenService;
            _userInfoService = userInfoService;
            _invoiceListService = invoiceListService;
            _invoiceDetailService = invoiceDetailService;
        }


        //public async Task<string> GetDataAsync(int currentPage = 0)
        //{
        //    var accessToken = await _tokenService.GetAccessTokenAsync();

        //}
    }
}
