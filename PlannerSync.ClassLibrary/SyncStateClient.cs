using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Text.Json;

namespace PlannerSync.ClassLibrary
{
    public class SyncStateClient
    {
        string storageConnectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
        string containerName = "syncstate";
        string blobName = "lastSyncState.json";

        public async Task<List<OutlookTask>> GetSavedSyncStateAsync()
        {
            List<OutlookTask> outlookTasks = new List<OutlookTask>();

            BlobContainerClient blobContainerClient = new BlobContainerClient(storageConnectionString, containerName);
            await blobContainerClient.CreateIfNotExistsAsync();

            BlobClient lastSyncStateBlobClient = blobContainerClient.GetBlobClient(blobName);
            if(lastSyncStateBlobClient.Exists())
            {
                BlobDownloadInfo lastSyncBlobDownloadInfo = await lastSyncStateBlobClient.DownloadAsync();
                StreamReader streamReader = new StreamReader(lastSyncBlobDownloadInfo.Content);
                string lastSyncTasks = streamReader.ReadToEnd();
                outlookTasks = JsonSerializer.Deserialize<List<OutlookTask>>(lastSyncTasks);
            }

            return outlookTasks;
        }
        
        public async Task SaveSyncStateAsync(List<OutlookTask> outlookTasks)
        {
            BlobContainerClient blobContainerClient = new BlobContainerClient(storageConnectionString, containerName);
            await blobContainerClient.CreateIfNotExistsAsync();

            string outlookTasksJson = JsonSerializer.Serialize(outlookTasks);
            byte[] outlookTasksByteArray = Encoding.UTF8.GetBytes(outlookTasksJson);
            MemoryStream outlookTasksStream = new MemoryStream(outlookTasksByteArray);
            
            BlobClient lastSyncStateBlobClient = blobContainerClient.GetBlobClient(blobName);
            lastSyncStateBlobClient.DeleteIfExists();
            lastSyncStateBlobClient.Upload(outlookTasksStream);
        }
        
    }
}
