public class BaseConfigurationSettings
{
    public const string Section = "BaseConfiguration";
    public required string WorkingDirectory { get; set; }
    public required string OutputDirectory { get; set; }
    public required IEnumerable<Proxy> Proxies { get; set; }
    public required CBZPackager CBZPackagerConfig { get; set; }
}

public class Proxy
{
    public required IEnumerable<string> IPAddress { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
}

public class CBZPackager
{
    public required int MaxAttempts { get; set; }
}
