using System.IO;
using System.Threading.Tasks;

namespace HospitalRegistrationSystem.Application.Interfaces;

public interface IFileStorageManager
{
    public Task<string> UploadAsync(Stream content, string directory, string fileName);
    public Task<Stream> DownloadAsync(string path);
}
