using Azure.Storage.Blobs.Models;

namespace AssignmentNo4.ViewModels {
    public class IndexModel {
        public Azure.Pageable<BlobContainerItem> Containers { get; set; }
    }
}