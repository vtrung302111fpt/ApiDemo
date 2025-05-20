using System.Text.Json;
using API_Project1.Models;

namespace API_Project1.Interfaces
{
    public interface IInvoiceListService
    {
        Task<string> GetInvoiceListAsync(int currentPage);
        Task<List<string>> GetMaHoaDonListAsync(int currentPage);

        //Task<JsonElement> GetInvoiceListDataAsync(int currentPage);

        Task SaveToDatabaseAsync(List<InvoiceListDataModel> invoices);

        List<InvoiceListDataModel> ConvertJsonToInvoiceList(JsonElement data);
    }
}
