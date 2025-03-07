using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using ManagedPnp.Avalonia.Core.Results;
using ManagedPnp.Avalonia.Core.Results.Errors;

namespace ManagedPnp.Avalonia.Core.PsClient;

public class PnpUtilClient : IPnpUtilClient
{

    public async Task<Result<string>> FireCommand(PnpUtilParam mainParam, params (PnpUtilParam?, string?)[] extraParams)
    {
        try
        {
            using var shell = PowerShell.Create();

            shell.AddCommand(PsCommand.PnpUtil)
                .AddArgument(mainParam);


            if (extraParams.Length > 0)
            {
                foreach ((PnpUtilParam?, string?) extraParam in extraParams)
                {
                    shell.AddArgument(extraParam.Item1);
                    shell.AddArgument(extraParam.Item2);
                }

            }

            shell.AddCommand(PsCommand.SelectObject)
                .AddParameter("Skip", 1);
            shell.AddCommand(PsCommand.OutString);
            
            var outputList = new PSDataCollection<string>();
            var errors = new List<IError>();

            shell.Streams.Error.DataAdded += (sender, args) =>
            {
                errors.Add(PowerShellErrors.RuntimeError(((PSDataCollection<ErrorRecord>)sender!)[args.Index]));
            };

            await shell.InvokeAsync<PSObject, string>(null, outputList);

            return errors.Count > 0
                ? PowerShellErrors.CompositeError(errors, "Runtime Errors Encountered")
                : outputList.First().Trim();
        }
        catch (Exception e)
        {
            return PowerShellErrors.RuntimeException(e);
        }
    }
}