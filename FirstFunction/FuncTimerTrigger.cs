using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace FirstFunction;

public static class FuncTimerTrigger
{
    [FunctionName("FuncTimerTrigger")]
    public static void Run([TimerTrigger("* * * * * *")] TimerInfo myTimer, 
        ILogger log, 
        [Blob("logs/{rand-guid}.txt", System.IO.FileAccess.Write, Connection = "MyAzureStorageBlob")] 
        Stream blobStream)
    {
        var ifade = Encoding.UTF8.GetBytes($"loglama: {DateTime.Now}");

        blobStream.Write(ifade, 0, ifade.Length);
        blobStream.Close();
        blobStream.Dispose();

        log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");        
    }
}