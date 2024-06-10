namespace HospitalRegistrationSystem.Infrastructure;

public class AzureBlobOptions
{
    public const string SectionName = "AzureBlob";

    public required string ContainerName { get; set; }
}