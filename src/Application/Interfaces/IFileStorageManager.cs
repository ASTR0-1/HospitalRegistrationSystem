using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalRegistrationSystem.Application.Interfaces;

public interface IFileStorageManager
{
    public Task<string> UploadAsync(Stream content, string directory, string fileName);
    public Task<Stream> DownloadAsync(string path);
}
