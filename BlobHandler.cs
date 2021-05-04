using System;
using Azure.Storage.Blobs;

// Examples from: https://docs.microsoft.com/en-us/dotnet/api/overview/azure/storage.blobs-readme

namespace ApiPatternsClient
{
    class BlobHandler
    {

        public static string uploadFile(string connectionString)
        {
            string containerName = "sample-container";
            string blobName = "GitMsftLogo.png";
            string filePath = "./ExampleFiles/GitMsftLogo.png";
            

            // Get a reference to a container named "sample-container" 
            BlobContainerClient container = new BlobContainerClient(connectionString, containerName);
            //container.Create();

            // Get a reference to a blob named "GitMsftLogo.png" in a container named "sample-container"
            BlobClient blob = container.GetBlobClient(blobName);

            // Upload local file
            try
            {
                Azure.Response<Azure.Storage.Blobs.Models.BlobContentInfo> results = blob.Upload(filePath);
                return results.ToString();
                //201 = Success
            }
            catch (System.Exception ex)
            {
                return ex.Message.ToString();
                //409 = Already exists
            }

        }

        public static string downloadFile(string connectionString)
        {
            // Get a temporary path on disk where we can download the file
            string containerName = "sample-container";
            string blobName = "GitMsftLogo.png";
            string downloadPath = "./ExampleFiles/downloaded.png";

            BlobContainerClient container = new BlobContainerClient(connectionString, containerName);
            BlobClient blobClient = container.GetBlobClient(blobName);

            try
            {
                Azure.Response results = blobClient.DownloadTo(downloadPath);
                return results.ToString();
                //206 - Success
            }
            catch (System.Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}
