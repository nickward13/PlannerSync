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
    internal class BlobSyncStateClient : ISyncStateClient
    {
        string storageConnectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
        string containerName = "syncstate";
        string blobName = "lastSyncState.json";

        public async Task<List<SyncedTask>> GetSavedSyncStateAsync()
        {
            List<SyncedTask> syncedTasks = new List<SyncedTask>();

            BlobContainerClient blobContainerClient = new BlobContainerClient(storageConnectionString, containerName);
            await blobContainerClient.CreateIfNotExistsAsync();

            BlobClient lastSyncStateBlobClient = blobContainerClient.GetBlobClient(blobName);
            if(lastSyncStateBlobClient.Exists())
            {
                BlobDownloadInfo lastSyncBlobDownloadInfo = await lastSyncStateBlobClient.DownloadAsync();
                StreamReader streamReader = new StreamReader(lastSyncBlobDownloadInfo.Content);
                string lastSyncTasks = streamReader.ReadToEnd();
                syncedTasks = JsonSerializer.Deserialize<List<SyncedTask>>(lastSyncTasks);
            }

            return syncedTasks;
        }
        
        public async Task SaveSyncStateAsync(List<SyncedTask> syncedTasks)
        {
            BlobContainerClient blobContainerClient = new BlobContainerClient(storageConnectionString, containerName);
            await blobContainerClient.CreateIfNotExistsAsync();

            string outlookTasksJson = JsonSerializer.Serialize(syncedTasks);
            byte[] outlookTasksByteArray = Encoding.UTF8.GetBytes(outlookTasksJson);
            MemoryStream outlookTasksStream = new MemoryStream(outlookTasksByteArray);
            
            BlobClient lastSyncStateBlobClient = blobContainerClient.GetBlobClient(blobName);
            lastSyncStateBlobClient.DeleteIfExists();
            lastSyncStateBlobClient.Upload(outlookTasksStream);
        }
        
    }
}
