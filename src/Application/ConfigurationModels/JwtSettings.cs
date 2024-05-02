namespace HospitalRegistrationSystem.Application.ConfigurationModels;

public class JwtSettings
{
    public const string SectionName = "JwtSettings";

    public string? Secret { get; set; }
    public string? ValidIssuer { get; set; }
    public string? ValidAudience { get; set; }
    public int Expires { get; set; }
}
