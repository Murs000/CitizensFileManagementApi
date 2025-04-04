using CitizenFileManagement.Core.Domain.Common;
using CitizenFileManagement.Core.Domain.Enums;

namespace CitizenFileManagement.Core.Domain.Entities;

public class FilePack : BaseEntity<FilePack>
{
    public string Name { get; set; }
    public string? Description { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public ICollection<Document> Documents { get; set; } 

    public FilePack SetDetails(string name, string? description)
    {
        Name = name;
        Description = description;

        return this;
    }
    public FilePack SetUser(int userId)
    {
        UserId = userId;

        return this;
    }   
}