using SeconAPI.Domain.Entities;

namespace SeconAPI.Application.Interfaces.Services;

public interface IExcelParser
{
    Task<List<MeterData>> GetMeterDataByDocumentAsync(byte[] excelReport);
    Task<List<MeterData>> GetMeterDataByDocumentAsync(List<byte[]> excelReport);
    
    Task<byte[]> EditExcelReportAsync(byte[] excelData, List<MeterData> unmatchedMeters);

}