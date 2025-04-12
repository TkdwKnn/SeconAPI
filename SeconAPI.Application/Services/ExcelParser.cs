using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Style;
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
    
    
    
    public async Task<byte[]> EditExcelReportAsync(byte[] excelData, List<MeterData> allMeterData)
    {
        using var package = new ExcelPackage(new MemoryStream(excelData));
    
        foreach (var worksheet in package.Workbook.Worksheets)
        {
            var meterNumberCol = GetColumnIndex(worksheet, "MeterNumber");
            var meterTypeCol = GetColumnIndex(worksheet, "MeterType");
        
            if (meterNumberCol == -1 || meterTypeCol == -1) continue;

            for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
            {
                var currentNumber = worksheet.Cells[row, meterNumberCol].Text;
                var currentType = worksheet.Cells[row, meterTypeCol].Text;

                var meter = allMeterData.FirstOrDefault(m => 
                    m.MeterNumber == currentNumber && 
                    m.MeterType == currentType);

                if (meter != null && !meter.IsMatched)
                {
                    SetRedBackground(worksheet.Cells[row, meterNumberCol]);
                    SetRedBackground(worksheet.Cells[row, meterTypeCol]);
                }
            }
        }
    
        return package.GetAsByteArray();
    }

    private void SetRedBackground(ExcelRange cell)
    {
        cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
        cell.Style.Fill.BackgroundColor.SetColor(Color.Red);
        cell.Style.Font.Bold = true;
    }
    
    
    
    
    
    

    private int GetColumnIndex(ExcelWorksheet worksheet, string columnName)
    {
        for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
        {
            if (worksheet.Cells[1, col].Text.Equals(columnName, StringComparison.OrdinalIgnoreCase))
                return col;
        }
        return -1;
    }
}