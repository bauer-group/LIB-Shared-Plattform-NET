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
        protected Cloudinary Client { get; private set; } = null!;
        public CloudinaryImageManager(CloudinaryImageManagerConfiguration configuration)
        {
            Configuration = configuration;

            Initalize();
        }

        private void Initalize()
        {
            var account = new Account(Configuration.Name, Configuration.APIKey, Configuration.APISecret);
            Client = new Cloudinary(account);
        }

        public void Dispose()
        {

        }

        public String ToCloudinaryUID(Guid uniqueIdentifier)
        {
            return String.Format("{0}/{1:N}", Configuration.Project, uniqueIdentifier);
        }

        public String UploadImage(String filenameOrUrl, Guid uniqueIdentifier, List<Transformation>? transformations = null)
        {
            return UploadImage(filenameOrUrl, uniqueIdentifier.ToString("N"), transformations);
        }

        public String UploadImage(String filenameOrUrl, String uniqueIdentifier, List<Transformation>? transformations = null)
        {
            var uploadParameters = new ImageUploadParams()
            {
                File = new FileDescription(filenameOrUrl),
                Folder = Configuration.Project,
                PublicId = uniqueIdentifier,

                EagerTransforms = transformations,

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

        public String UploadVideo(String filenameOrUrl, String uniqueIdentifier)
        {
            var uploadParameters = new VideoUploadParams()
            {
                File = new FileDescription(filenameOrUrl),
                Folder = Configuration.Project,
                PublicId = uniqueIdentifier,

                ImageMetadata = true,

                Invalidate = true
            };

            var uploadResult = Client.Upload(uploadParameters);

            return uploadResult.PublicId;
        }

        public Boolean Delete(Guid uniqueIdentifier)
        {
            return Delete(ToCloudinaryUID(uniqueIdentifier));
        }

        public Boolean Delete(String uniqueIdentifier)
        {
            var deleteParameters = new DelResParams()
            {
                PublicIds = new List<String>() { uniqueIdentifier },
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

        public Resource[] List(String? uniqueIdentifier = null, CloudinaryContentType contentType = CloudinaryContentType.Image)
        {
            var listParameters = new ListResourcesParams()
            {
                MaxResults = 500,
                ResourceType = contentType == CloudinaryContentType.Image ? ResourceType.Image : ResourceType.Video
            };

            var listResult = String.IsNullOrWhiteSpace(uniqueIdentifier) ? Client.ListResources(listParameters) : Client.ListResourcesByPublicIds(new[] { uniqueIdentifier });

            var existingRessources = listResult.Resources;
            return existingRessources;
        }

        public Resource[] List(Guid uniqueIdentifier, CloudinaryContentType contentType = CloudinaryContentType.Image)
        {
            return List(ToCloudinaryUID(uniqueIdentifier), contentType);
        }

        public UsageResult ServiceUsage()
        {
            return Client.GetUsage();
        }

        public Url GetImageTransformation(Transformation transformation)
        {
            return Client.Api.UrlImgUp.Transform(transformation);
        }

        public Url GetImageTransformation(UInt16 width, UInt16 height, Boolean crop = true, CloudinaryImageFormat format = CloudinaryImageFormat.Original)
        {
            var transformation = new Transformation().Width(width).Height(height);

            switch (format)
            {
                case CloudinaryImageFormat.PNG:
                    transformation = transformation.FetchFormat("png");
                    break;

                case CloudinaryImageFormat.JPEG:
                    transformation = transformation.FetchFormat("jpg");
                    break;
            }

            return GetImageTransformation(crop ? transformation.Crop("fill") : transformation);
        }

        public String ToURL(Url transformation, String uniqueIdentifier, Boolean useSSL = true)
        {
            return transformation.Secure(useSSL).BuildUrl(uniqueIdentifier);
        }

        public String ToImageTag(Url transformation, String uniqueIdentifier, StringDictionary? parameters = null, Boolean useSSL = true)
        {
            return transformation.Secure(useSSL).BuildImageTag(uniqueIdentifier, parameters);
        }

        public String ToURL(String uniqueIdentifier, Boolean useSSL = true)
        {
            return Client.Api.UrlImgUp.Secure(useSSL).BuildUrl(uniqueIdentifier);
        }

        public String ToImageTag(String uniqueIdentifier, StringDictionary? parameters = null, Boolean useSSL = true)
        {
            return Client.Api.UrlImgUp.Secure(useSSL).BuildImageTag(uniqueIdentifier, parameters);
        }

        public String ToURL(Url transformation, Guid uniqueIdentifier, Boolean useSSL = true)
        {
            return ToURL(transformation, ToCloudinaryUID(uniqueIdentifier), useSSL);
        }

        public String ToImageTag(Url transformation, Guid uniqueIdentifier, StringDictionary? parameters = null, Boolean useSSL = true)
        {
            return ToImageTag(transformation, ToCloudinaryUID(uniqueIdentifier), parameters, useSSL);
        }

        public String ToURL(Guid uniqueIdentifier, Boolean useSSL = true)
        {
            return ToURL(ToCloudinaryUID(uniqueIdentifier), useSSL);
        }

        public String ToImageTag(Guid uniqueIdentifier, StringDictionary? parameters = null, Boolean useSSL = true)
        {
            return ToImageTag(ToCloudinaryUID(uniqueIdentifier), parameters, useSSL);
        }

        public static StringDictionary GetImageTagParameters(String name)
        {
            var parameters = new StringDictionary();
            parameters.Add(new KeyValuePair<String, String>("alt", name));

            return parameters;
        }
    }
}
