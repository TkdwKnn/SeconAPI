using OfficeOpenXml;
using SeconAPI.Application.Interfaces.Services;
using SeconAPI.Domain.Entities;

namespace SeconAPI.Application.Services;

public class ExcelParser : IExcelParser
{

    public ExcelParser()
    {
     
    }
    
    public async Task<List<MeterData>> GetMeterDataByDocumentAsync(byte[] excelReport)
    {
        var meterDatas = new List<MeterData>();

        await Task.Run(() =>
        {
            using MemoryStream ms = new MemoryStream(excelReport);
            using ExcelPackage package = new ExcelPackage(ms);

            var worksheet = package.Workbook.Worksheets[0];

            var rows = worksheet.Dimension.Rows;

            for (var row = 0; row < rows; row++)
            {
                var meterData = new MeterData()
                {
                    City = worksheet.Cells[row, 3].GetValue<string>(),
                    Street = worksheet.Cells[row, 4].GetValue<string>(),
                    Apartment = worksheet.Cells[row, 5].GetValue<string>(),
                    MeterType = worksheet.Cells[row, 12].GetValue<string>(),
                    MeterNumber = worksheet.Cells[row, 13].GetValue<string>(),
                    IsMatched = false
                };

                meterDatas.Add(meterData);
            }
        });
        return meterDatas;
    }
}