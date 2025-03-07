using System.Threading.Tasks;
using ManagedPnp.Avalonia.Core.Results;

namespace ManagedPnp.Avalonia.Core.Permanence;

public interface IPermanenceService
{
    public Task<Result<T>> SaveAsync<T>(T model);
    public Task<Result<T>> Load<T>();
}