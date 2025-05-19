namespace API_Project1.Interfaces
{
    public interface IInvoiceDetailService
    {
        Task<string> GetInvoiceDetailAsync(int currentPage);
    }
}
