namespace SeconAPI.Domain.Entities;

public class MeterData
{
    public string? City { get; set; }        
    public string? Street { get; set; }      
    public string? Building { get; set; }      
    public string? Apartment { get; set; }     
    public string? MeterType { get; set; }  
    public string? MeterNumber { get; set; }
    public bool IsMatched { get; set; } = false;
    
    
    public string GetNewFileName()
    {
        return $"{City}_{Street}_{Building}_{Apartment}_{MeterType}_{MeterNumber}";
    }
    
}