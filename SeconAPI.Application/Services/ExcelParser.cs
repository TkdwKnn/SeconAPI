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

            for (var row = 10; row < rows; row++)
            {
                
                
                var oldMeterData = new MeterData()
                {
                    City = worksheet.Cells[row, 4].GetValue<string>(),
                    Street = worksheet.Cells[row, 5].GetValue<string>(),
                    Building = worksheet.Cells[row, 6].GetValue<string>(),
                    Apartment = worksheet.Cells[row, 7].GetValue<string>(),
                    MeterType = worksheet.Cells[row, 11].GetValue<string>(),
                    MeterNumber = worksheet.Cells[row, 12].GetValue<string>(),
                    IsNew = false,
                    IsMatched = false
                };


                var newMeterData = new MeterData()
                {
                    City = worksheet.Cells[row, 4].GetValue<string>(),
                    Street = worksheet.Cells[row, 5].GetValue<string>(),
                    Building = worksheet.Cells[row, 6].GetValue<string>(),
                    Apartment = worksheet.Cells[row, 7].GetValue<string>(),
                    MeterType = worksheet.Cells[row, 14].GetValue<string>(),
                    MeterNumber = worksheet.Cells[row, 15].GetValue<string>(),
                    IsNew = true,
                    IsMatched = false
                };

                meterDatas.Add(oldMeterData);
                meterDatas.Add(newMeterData);
                
            }   
        });
        return meterDatas;
    }

    public async Task<List<MeterData>> GetMeterDataByDocumentAsync(List<byte[]> excelReport)
    {
        var meterDatas = new List<MeterData>();

        foreach (var file in excelReport)
        {
            meterDatas.AddRange(await GetMeterDataByDocumentAsync(file));
        }
        
        return meterDatas;
    }
}