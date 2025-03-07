using ManagedPnp.Avalonia.Core.Results.Errors;

namespace ManagedPnp.Avalonia.Core;

public static class ApplicationErrors
{
    public static IError DesignTestError => new Error("design.test.error",
        "This is not a real error, but a test error used in design time");
}