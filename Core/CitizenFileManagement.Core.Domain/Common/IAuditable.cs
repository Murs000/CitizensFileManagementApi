using System.Threading.Tasks.Dataflow;

namespace CitizenFileManagement.Core.Domain.Common;

public interface IAuditable<T> : ICreatable<T>, IEditable<T>
{
    public T SetCredentials(int userId);
}