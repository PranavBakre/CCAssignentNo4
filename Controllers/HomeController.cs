using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AssignmentNo4.Models;
using AssignmentNo4.Services.BlobService;
using Microsoft.AspNetCore.Http;
using AssignmentNo4.ViewModels;
using Azure.Storage.Blobs.Models;

namespace AssignmentNo4.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BlobService _blobService;
        public HomeController(
            ILogger<HomeController> logger,
            BlobService blobService)
        {
            _logger = logger;
            _blobService = blobService;
        }

        public IActionResult Index()
        {
            var containers = _blobService.GetContainers();
            
            return View(new IndexModel{
                Containers=containers
            });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        
        public async Task<Azure.Response<BlobContentInfo>> Submit(string containerName, IFormFile file){
            return await _blobService.UploadFileAsync(containerName,file);
            //Redirect("index");
        }

        public IEnumerable<BlobItem> GetContainerFiles(string containerName) {
            return _blobService.GetContainerFiles(containerName);
        }

        public async Task<IActionResult> DeleteFile(string containerName, string fileName) {
            
            var response = await _blobService.DeleteFileAsync(containerName,fileName);
            _logger.LogCritical(response.Status + " " + response.ReasonPhrase);
            return response.Status>=200 && response.Status<=299? Ok():BadRequest();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
