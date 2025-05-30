﻿using System.Text.Json;
using API_Project1.Models;

namespace API_Project1.Interfaces
{
    public interface IInvoiceListService
    {
        Task<string> GetInvoiceListAsync(int currentPage);
        Task<List<string>> GetMaHoaDonListAsync(int currentPage);

        Task SaveListToDatabaseAsync(List<InvoiceListDataModel> invoices);

        List<InvoiceListDataModel> ConvertJsonToInvoiceList(JsonElement data);
    }
}
