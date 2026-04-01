using Microsoft.Extensions.Configuration;

public class BaseUrl
{
    private readonly IConfiguration _config;

    public BaseUrl(IConfiguration config)
    {
        _config = config;
    }

    public string GetConnectionString()
    {
        var host = _config["DB_HOST"] ?? "localhost";
        var port = _config.GetValue<int>("DB_PORT", 5432);
        var name = _config["DB_NAME"] ?? "tradeflow";
        var user = _config["DB_USER"] ?? "postgres";
        var pass = _config["DB_PASS"] ?? "";

        return $"Host={host};Port={port};Database={name};Username={user};Password={pass}";
    }
}