namespace API_Project1.Interfaces
{
    public interface InterfaceInvoiceList
    {
        Task<string> GetInvoiceListAsync(int currentPage);
        Task<List<string>> GetMaHoaDonListAsync(int currentPage);
    }
}
