namespace CitizenFileManagement.Core.Application.Features.Queries.Models;

public class PaginationModel
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}