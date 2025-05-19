namespace API_Project1.Interfaces
{
    public interface IInvoiceListService
    {
        Task<string> GetInvoiceListAsync(int currentPage);
        Task<List<string>> GetMaHoaDonListAsync(int currentPage);
    }
}
