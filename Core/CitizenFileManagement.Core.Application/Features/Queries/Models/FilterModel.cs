namespace CitizenFileManagement.Core.Application.Features.Queries.Models;

public class FilterModel
{
    public string? SearchTerm { get; set; } 
    public DateTime? CreatedAfter { get; set; }
    public DateTime? CreatedBefore { get; set; }
}
