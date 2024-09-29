using CitizenFileManagement.Core.Domain.Enums;

namespace CitizenFileManagement.Core.Application.Features.Queries.Models;

public class ReturnItemModel<T>
{
    public IEnumerable<T> Items { get; set; }
    public int Count { get; set; }
}