using Microsoft.Extensions.Configuration;

namespace LegacyProcs.Helpers;

/// <summary>
/// Helper para leitura de configurações
/// ✅ MIGRADO: .NET 8 com IConfiguration
/// </summary>
public static class ConfigurationHelper
{
    private static IConfiguration? _configuration;

    public static void Initialize(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public static string GetConnectionString(string name = "DefaultConnection")
    {
        if (_configuration == null)
        {
            throw new InvalidOperationException("ConfigurationHelper não foi inicializado");
        }

        var connectionString = _configuration.GetConnectionString(name);
        
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException($"Connection string '{name}' não encontrada");
        }

        return connectionString;
    }

    public static string? GetAppSetting(string key, string? defaultValue = null)
    {
        if (_configuration == null)
        {
            return defaultValue;
        }

        return _configuration[key] ?? defaultValue;
    }
}
