using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAUERGROUP.Shared.Cloud.CloudinaryClient
{
    public class CloudinaryImageManager : IDisposable
    {
        protected CloudinaryImageManagerConfiguration Configuration { get; private set; }
        protected Cloudinary Client { get; private set; }
        public CloudinaryImageManager(CloudinaryImageManagerConfiguration oConfiguration)
        {
            Configuration = oConfiguration;

            Initalize();
        }

        private void Initalize()
        {
            var oAccount = new Account(Configuration.Name, Configuration.APIKey, Configuration.APISecret);
            Client = new Cloudinary(oAccount);
        }

        public void Dispose()
        {

        }

        public String ToCloudinaryUID(Guid gUniqueIdentifier)
        {
            return String.Format("{0}/{1:N}", Configuration.Project, gUniqueIdentifier);
        }

        public String UploadImage(String sFilenameOrURL, Guid gUniqueIdentifier, List<Transformation> oTransformations = null)
        {
            return UploadImage(sFilenameOrURL, gUniqueIdentifier.ToString("N"), oTransformations);
        }

        public String UploadImage(String sFilenameOrURL, String sUniqueIdentifier, List<Transformation> oTransformations = null)
        {
            var uploadParameters = new ImageUploadParams()
            {
                File = new FileDescription(sFilenameOrURL),
                Folder = Configuration.Project,
                PublicId = sUniqueIdentifier,

                EagerTransforms = oTransformations,

                ImageMetadata = true,
                Exif = true,
                Colors = true,

                Invalidate = true
            };

            var uploadResult = Client.Upload(uploadParameters);
            if (uploadResult.Error != null)
                throw new CloudinaryException(uploadResult.StatusCode, uploadResult.Error.Message);

            return uploadResult.Url.AbsoluteUri;
        }

        public String UploadVideo(String sFilenameOrURL, String sUniqueIdentifier)
        {
            var uploadParameters = new VideoUploadParams()
            {
                File = new FileDescription(sFilenameOrURL),
                Folder = Configuration.Project,
                PublicId = sUniqueIdentifier,

                ImageMetadata = true,

                Invalidate = true
            };

            var uploadResult = Client.Upload(uploadParameters);

            return uploadResult.PublicId;
        }

        public Boolean Delete(Guid gUniqueIdentifier)
        {
            return Delete(ToCloudinaryUID(gUniqueIdentifier));
        }

        public Boolean Delete(String sUniqueIdentifier)
        {
            var deleteParameters = new DelResParams()
            {
                PublicIds = new List<String>() { sUniqueIdentifier },
                Invalidate = true,
                KeepOriginal = false,
            };

            var deleteResult = Client.DeleteResources(deleteParameters);
            return deleteResult.Partial == false;
        }

        public Boolean DeleteAll()
        {
            var deleteParameters = new DelResParams()
            {
                Prefix = Configuration.Project,
                Invalidate = true,
                KeepOriginal = false,
            };

            var deleteResult = Client.DeleteResources(deleteParameters);
            return deleteResult.Partial == false;
        }

        public Resource[] List(String sUniqueIdentifier = null, CloudinaryContentType eType = CloudinaryContentType.Image)
        {
            var listParameters = new ListResourcesParams()
            {
                MaxResults = 500,
                ResourceType = eType == CloudinaryContentType.Image ? ResourceType.Image : ResourceType.Video
            };

            var listResult = String.IsNullOrWhiteSpace(sUniqueIdentifier) ? Client.ListResources(listParameters) : Client.ListResourcesByPublicIds(new[] { sUniqueIdentifier });

            var existingRessources = listResult.Resources;
            return existingRessources;
        }

        public Resource[] List(Guid gUniqueIdentifier, CloudinaryContentType eType = CloudinaryContentType.Image)
        {
            return List(ToCloudinaryUID(gUniqueIdentifier), eType);
        }

        public UsageResult ServiceUsage()
        {
            return Client.GetUsage();
        }

        public Url GetImageTransformation(Transformation oTransformation)
        {
            return Client.Api.UrlImgUp.Transform(oTransformation);
        }

        public Url GetImageTransformation(UInt16 iWidth, UInt16 iHeight, Boolean bCrop = true, CloudinaryImageFormat eFormat = CloudinaryImageFormat.Original)
        {
            var oTransformation = new Transformation().Width(iWidth).Height(iHeight);

            switch (eFormat)
            {
                case CloudinaryImageFormat.PNG:
                    oTransformation = oTransformation.FetchFormat("png");
                    break;

                case CloudinaryImageFormat.JPEG:
                    oTransformation = oTransformation.FetchFormat("jpg");
                    break;
            }

            return GetImageTransformation(bCrop ? oTransformation.Crop("fill") : oTransformation);
        }

        public String ToURL(Url oTransformation, String sUniqueIdentifier, Boolean bSSL = true)
        {
            return oTransformation.Secure(bSSL).BuildUrl(sUniqueIdentifier);
        }

        public String ToImageTag(Url oTransformation, String sUniqueIdentifier, StringDictionary oParameters = null, Boolean bSSL = true)
        {
            return oTransformation.Secure(bSSL).BuildImageTag(sUniqueIdentifier, oParameters);
        }

        public String ToURL(String sUniqueIdentifier, Boolean bSSL = true)
        {
            return Client.Api.UrlImgUp.Secure(bSSL).BuildUrl(sUniqueIdentifier);
        }

        public String ToImageTag(String sUniqueIdentifier, StringDictionary oParameters = null, Boolean bSSL = true)
        {
            return Client.Api.UrlImgUp.Secure(bSSL).BuildImageTag(sUniqueIdentifier, oParameters);
        }

        public String ToURL(Url oTransformation, Guid gUniqueIdentifier, Boolean bSSL = true)
        {
            return ToURL(oTransformation, ToCloudinaryUID(gUniqueIdentifier), bSSL);
        }

        public String ToImageTag(Url oTransformation, Guid gUniqueIdentifier, StringDictionary oParameters = null, Boolean bSSL = true)
        {
            return ToImageTag(oTransformation, ToCloudinaryUID(gUniqueIdentifier), oParameters, bSSL);
        }

        public String ToURL(Guid gUniqueIdentifier, Boolean bSSL = true)
        {
            return ToURL(ToCloudinaryUID(gUniqueIdentifier), bSSL);
        }

        public String ToImageTag(Guid gUniqueIdentifier, StringDictionary oParameters = null, Boolean bSSL = true)
        {
            return ToImageTag(ToCloudinaryUID(gUniqueIdentifier), oParameters, bSSL);
        }

        public static StringDictionary GetImageTagParameters(String sName)
        {
            var oParameters = new StringDictionary();
            oParameters.Add(new KeyValuePair<String, String>("alt", sName));

            return oParameters;
        }
    }
}
