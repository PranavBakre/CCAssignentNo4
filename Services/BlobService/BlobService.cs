using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AssignmentNo4.Services.BlobService {
    public class BlobService {

        private readonly BlobServiceClient _blobServiceClient;
        private readonly ILogger<BlobService> _logger;
        public BlobService(
            BlobServiceClient blobServiceClient,
            ILogger<BlobService> logger
            ) {
            _blobServiceClient=blobServiceClient;
            _logger=logger;
        }

        public async Task CreateContainerAsync(string cotainerName) => 
            await _blobServiceClient.CreateBlobContainerAsync(
                blobContainerName:cotainerName,
                publicAccessType:PublicAccessType.BlobContainer);

        public async Task DeleteContainerAsync(string containerName) => 
            await _blobServiceClient.DeleteBlobContainerAsync(
                blobContainerName:containerName);

        public async Task<Azure.Response<BlobContentInfo>> UploadFileAsync(string containerName, IFormFile file) {
            var container = _blobServiceClient.GetBlobContainerClient(containerName);
            return await container.UploadBlobAsync(file.FileName,file.OpenReadStream());
        }

        public async Task<Azure.Response> DeleteFileAsync(string containerName, string fileName) {
            _logger.LogCritical(containerName+" "+fileName);
            var container = _blobServiceClient.GetBlobContainerClient(containerName);
            return await container.DeleteBlobAsync(fileName);
        }
            
        public Azure.Pageable<BlobContainerItem> GetContainers(){
             return _blobServiceClient.GetBlobContainers();
        }

        public Azure.Pageable<BlobItem> GetContainerFiles(string containerName) {
            var container = _blobServiceClient.GetBlobContainerClient(containerName);
            return container.GetBlobs();
        }
    }
}