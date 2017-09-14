using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTX.Bot.ConexaoLiq.Storage
{
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;

    namespace BlobStorageDemo
    {
        public class ImageService
        {
            public async Task<string> UploadImageAsync(string sourceUrl)
            {

 
                string imageFullPath = null;
            
                try
                {
                    CloudStorageAccount cloudStorageAccount = ConnectionString.GetConnectionString();
                    CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                    CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("fotos");

                    if (await cloudBlobContainer.CreateIfNotExistsAsync())
                    {
                        await cloudBlobContainer.SetPermissionsAsync(
                            new BlobContainerPermissions
                            {
                                PublicAccess = BlobContainerPublicAccessType.Blob
                            }
                            );
                    }
                    string imageName = Guid.NewGuid().ToString() + ".jpg";

                    CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(imageName);

                    await cloudBlockBlob.StartCopyAsync(new Uri(sourceUrl));

                    imageFullPath = cloudBlockBlob.Uri.ToString();
                }
                catch (Exception ex)
                {

                }
                return imageFullPath;
            }
        }
    }
}