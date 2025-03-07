using System.Threading.Tasks;
using ManagedPnp.Avalonia.Core.Results;

namespace ManagedPnp.Avalonia.Core.PsClient;

public interface IPnpUtilClient
{
    Task<Result<string>> FireCommand(PnpUtilParam mainParam, params (PnpUtilParam?, string?)[] extraParams);
}