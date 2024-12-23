using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;

namespace FirstFunction;

public class ResizeBlobTrigger
{
    [FunctionName("ResizeBlobTrigger")]
    public static async Task Run(
        [BlobTrigger("source-container/{name}", Connection = "AzureWebJobsStorage")] Stream inputBlob, 
        string name,
        ILogger log,
        [Blob("resized-container/{name}", FileAccess.Write, Connection = "AzureWebJobsStorage")] BlobClient outputBlobClient)
    {
        IImageFormat format = await Image.DetectFormatAsync(inputBlob);
        using var image = Image.Load(inputBlob);

        image.Mutate(x => x.Resize(new ResizeOptions
        {
            Mode = ResizeMode.Max, 
            Size = new Size(100, 100) 
        }));

        using var outputStream = new MemoryStream();
        image.Save(outputStream, format);
        outputStream.Position = 0; 

        await outputBlobClient.UploadAsync(outputStream, overwrite: true);

        log.LogInformation($"Resim resize işlemi başarıyla gerçekleştirildi.");        
    }
}