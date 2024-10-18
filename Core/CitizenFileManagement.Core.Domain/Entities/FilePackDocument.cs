using System;

namespace CitizenFileManagement.Core.Domain.Entities;

public class FilePackDocument : BaseEntity<FilePackDocument>
{  
    public int FilePackId { get; set; }    // Foreign Key
    public FilePack FilePack { get; set; }

    public int DocumentId { get; set; }    // Foreign Key
    public Document Document { get; set; }

    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
}