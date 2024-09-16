using System.Threading.Tasks.Dataflow;

namespace CitizenFileManagement.Core.Domain.Common;

public interface IAuditable : ICreatable, IEditable
{
    public void SetCredentials(int userId);
}