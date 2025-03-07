using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using ManagedPnp.Avalonia.Core.Results;
using ManagedPnp.Avalonia.Core.Results.Errors;

namespace ManagedPnp.Avalonia.Core.Permanence;

public record DiskPermanenceConfiguration(string PermanenceFilePath);

public class DiskPermanenceService : IPermanenceService
{
    private readonly DiskPermanenceConfiguration _configuration;


    public DiskPermanenceService(DiskPermanenceConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    
    public async Task<Result<T>> SaveAsync<T>(T model)
    {
        return await WriteToFile(_configuration.PermanenceFilePath, model);
    }

    public async Task<Result<T>> Load<T>()
    {
        return await ReadFromFile<T>(_configuration.PermanenceFilePath);
    }

    private static async Task<Result<T>> WriteToFile<T>(string filePath, T data)
    {
        string tempFilePath = filePath + ".tmp";

        byte[] json = JsonSerializer.SerializeToUtf8Bytes(data);

        try
        {
            await File.WriteAllBytesAsync(tempFilePath, json);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            File.Move(tempFilePath, filePath);

            return data;
        }
        catch (UnauthorizedAccessException ex)
        {
            return new ExceptionError("file.exception.unauthorized", ex);
        }
        catch (IOException ex)
        {
            return new ExceptionError("file.exception.systemio", ex);
        }
    }

    private static async Task<Result<T>> ReadFromFile<T>(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                return new Error("file.read.notfound", "File couldn't be found");
            }

            await using FileStream fileStream = File.OpenRead(filePath);
            var result = await JsonSerializer.DeserializeAsync<T>(fileStream);
            if (result is null)
            {
                return new Error("file.model.notfound", "Model couldn't be read from json");
            }

            return result;

        }
        catch (UnauthorizedAccessException ex)
        {
            return new ExceptionError("file.exception.unauthorized", ex);
        }
        catch (IOException ex)
        {
            return new ExceptionError("file.exception.systemio", ex);
        }
    }
}