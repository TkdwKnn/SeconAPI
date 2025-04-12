using SeconAPI.Domain.Entities;

namespace SeconAPI.Application.Interfaces.Services;

public interface IExcelParser
{
    Task<List<MeterData>> GetMeterDataByDocumentAsync(byte[] excelReport);
    
}