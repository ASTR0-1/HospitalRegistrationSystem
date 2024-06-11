using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using HospitalRegistrationSystem.Application.Interfaces;
using Microsoft.Extensions.Options;

namespace HospitalRegistrationSystem.Infrastructure.Services;

/// <summary>
///     Azure blob file storage manager to upload and download files.
/// </summary>
public class AzureBlobFileStorageManager : IFileStorageManager
{
    private readonly AzureBlobOptions _azureBlobOptions;
    private readonly BlobServiceClient _serviceClient;

    /// <summary>
    ///     AzureBlobFileStorageManager constructor.
    /// </summary>
    /// <param name="serviceClient">Blob service client.</param>
    /// <param name="azureBlobOptions">IOptions to get azure blob options.</param>
    public AzureBlobFileStorageManager(BlobServiceClient serviceClient, IOptions<AzureBlobOptions> azureBlobOptions)
    {
        _serviceClient = serviceClient;
        _azureBlobOptions = azureBlobOptions.Value;
    }

    /// <summary>
    ///     Uploads a file to the azure blob storage.
    /// </summary>
    /// <param name="content">File content.</param>
    /// <param name="directory">Directory name in azure blob.</param>
    /// <param name="fileName">File name in azure blob.</param>
    /// <returns>Uri string to the uploaded file.</returns>
    public async Task<string> UploadAsync(Stream content, string directory, string fileName)
    {
        var blobContainerClient = _serviceClient.GetBlobContainerClient(_azureBlobOptions.ContainerName);
        var blobClient = blobContainerClient.GetBlobClient($"{directory}/{fileName}");

        content.Position = 0;
        await blobClient.UploadAsync(content);

        return blobClient.Uri.ToString();
    }

    /// <summary>
    ///     Downloads a file from the azure blob storage.
    /// </summary>
    /// <param name="path">Path to the file in azure blob.</param>
    /// <returns>Stream content of the file.</returns>
    public async Task<Stream> DownloadAsync(string path)
    {
        var blobContainerClient = _serviceClient.GetBlobContainerClient(_azureBlobOptions.ContainerName);
        var blobClient = blobContainerClient.GetBlobClient(path);

        var info = await blobClient.DownloadAsync();

        return info.Value.Content;
    }
}