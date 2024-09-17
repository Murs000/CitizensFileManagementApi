namespace CitizenFileManagement.Infrastructure.External.Settings;

public class JwtSettings
{
    public string Secret { get; set; }
    public string ExpireAt { get; set; }
}