using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAUERGROUP.Shared.Cloud.CloudinaryClient
{
    /// <summary>
    /// Manages image and video operations with the Cloudinary cloud service.
    /// </summary>
    /// <remarks>
    /// Provides functionality for uploading, deleting, listing, and transforming images and videos stored in Cloudinary.
    /// </remarks>
    public class CloudinaryImageManager : IDisposable
    {
        /// <summary>
        /// Gets the configuration settings for the Cloudinary connection.
        /// </summary>
        protected CloudinaryImageManagerConfiguration Configuration { get; private set; }

        /// <summary>
        /// Gets the underlying Cloudinary client instance.
        /// </summary>
        protected Cloudinary Client { get; private set; } = null!;

        /// <summary>
        /// Gets or sets the maximum number of results returned by list operations. Default is 500.
        /// </summary>
        public int MaxListResults { get; set; } = 500;

        /// <summary>
        /// Initializes a new instance of the <see cref="CloudinaryImageManager"/> class.
        /// </summary>
        /// <param name="configuration">The configuration containing Cloudinary credentials and project settings.</param>
        public CloudinaryImageManager(CloudinaryImageManagerConfiguration configuration)
        {
            Configuration = configuration;

            Initialize();
        }

        private void Initialize()
        {
            var account = new Account(Configuration.Name, Configuration.APIKey, Configuration.APISecret);
            Client = new Cloudinary(account);
        }

        /// <summary>
        /// Releases all resources used by the <see cref="CloudinaryImageManager"/>.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Converts a GUID to a Cloudinary-compatible unique identifier with project prefix.
        /// </summary>
        /// <param name="uniqueIdentifier">The GUID to convert.</param>
        /// <returns>A Cloudinary-formatted identifier string in the format "project/guid".</returns>
        public String ToCloudinaryUID(Guid uniqueIdentifier)
        {
            return String.Format("{0}/{1:N}", Configuration.Project, uniqueIdentifier);
        }

        /// <summary>
        /// Uploads an image to Cloudinary from a file path or URL.
        /// </summary>
        /// <param name="filenameOrUrl">The local file path or URL of the image to upload.</param>
        /// <param name="uniqueIdentifier">The GUID to use as the image's public identifier.</param>
        /// <param name="transformations">Optional list of transformations to apply eagerly during upload.</param>
        /// <returns>The absolute URL of the uploaded image.</returns>
        /// <exception cref="CloudinaryException">Thrown when the upload fails.</exception>
        public String UploadImage(String filenameOrUrl, Guid uniqueIdentifier, List<Transformation>? transformations = null)
        {
            return UploadImage(filenameOrUrl, uniqueIdentifier.ToString("N"), transformations);
        }

        /// <summary>
        /// Uploads an image to Cloudinary from a file path or URL.
        /// </summary>
        /// <param name="filenameOrUrl">The local file path or URL of the image to upload.</param>
        /// <param name="uniqueIdentifier">The string identifier to use as the image's public ID.</param>
        /// <param name="transformations">Optional list of transformations to apply eagerly during upload.</param>
        /// <returns>The absolute URL of the uploaded image.</returns>
        /// <exception cref="CloudinaryException">Thrown when the upload fails.</exception>
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

        /// <summary>
        /// Uploads a video to Cloudinary from a file path or URL.
        /// </summary>
        /// <param name="filenameOrUrl">The local file path or URL of the video to upload.</param>
        /// <param name="uniqueIdentifier">The string identifier to use as the video's public ID.</param>
        /// <returns>The public ID of the uploaded video.</returns>
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

        /// <summary>
        /// Deletes a resource from Cloudinary by its GUID.
        /// </summary>
        /// <param name="uniqueIdentifier">The GUID of the resource to delete.</param>
        /// <returns><c>true</c> if the deletion was successful; otherwise, <c>false</c>.</returns>
        public Boolean Delete(Guid uniqueIdentifier)
        {
            return Delete(ToCloudinaryUID(uniqueIdentifier));
        }

        /// <summary>
        /// Deletes a resource from Cloudinary by its public identifier.
        /// </summary>
        /// <param name="uniqueIdentifier">The public ID of the resource to delete.</param>
        /// <returns><c>true</c> if the deletion was successful; otherwise, <c>false</c>.</returns>
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

        /// <summary>
        /// Deletes all resources in the configured project folder from Cloudinary.
        /// </summary>
        /// <returns><c>true</c> if all deletions were successful; otherwise, <c>false</c>.</returns>
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

        /// <summary>
        /// Lists resources from Cloudinary, optionally filtered by identifier and content type.
        /// </summary>
        /// <param name="uniqueIdentifier">Optional public ID to filter by. If null, lists all resources.</param>
        /// <param name="contentType">The type of content to list (Image or Video).</param>
        /// <returns>An array of resources matching the criteria.</returns>
        public Resource[] List(String? uniqueIdentifier = null, CloudinaryContentType contentType = CloudinaryContentType.Image)
        {
            var listParameters = new ListResourcesParams()
            {
                MaxResults = MaxListResults,
                ResourceType = contentType == CloudinaryContentType.Image ? ResourceType.Image : ResourceType.Video
            };

            var listResult = String.IsNullOrWhiteSpace(uniqueIdentifier) ? Client.ListResources(listParameters) : Client.ListResourcesByPublicIds(new[] { uniqueIdentifier });

            var existingRessources = listResult.Resources;
            return existingRessources;
        }

        /// <summary>
        /// Lists resources from Cloudinary by GUID identifier.
        /// </summary>
        /// <param name="uniqueIdentifier">The GUID of the resource to find.</param>
        /// <param name="contentType">The type of content to list (Image or Video).</param>
        /// <returns>An array of resources matching the GUID.</returns>
        public Resource[] List(Guid uniqueIdentifier, CloudinaryContentType contentType = CloudinaryContentType.Image)
        {
            return List(ToCloudinaryUID(uniqueIdentifier), contentType);
        }

        /// <summary>
        /// Retrieves the current usage statistics for the Cloudinary account.
        /// </summary>
        /// <returns>The usage result containing bandwidth, storage, and request statistics.</returns>
        public UsageResult ServiceUsage()
        {
            return Client.GetUsage();
        }

        /// <summary>
        /// Gets a URL builder with the specified transformation applied.
        /// </summary>
        /// <param name="transformation">The transformation to apply.</param>
        /// <returns>A URL builder configured with the transformation.</returns>
        public Url GetImageTransformation(Transformation transformation)
        {
            return Client.Api.UrlImgUp.Transform(transformation);
        }

        /// <summary>
        /// Gets a URL builder with width, height, crop, and format transformations.
        /// </summary>
        /// <param name="width">The target width in pixels.</param>
        /// <param name="height">The target height in pixels.</param>
        /// <param name="crop">Whether to crop the image to fit the dimensions.</param>
        /// <param name="format">The output format for the image.</param>
        /// <returns>A URL builder configured with the specified transformations.</returns>
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

        /// <summary>
        /// Builds a URL for an image with the specified transformation.
        /// </summary>
        /// <param name="transformation">The URL builder with transformations.</param>
        /// <param name="uniqueIdentifier">The public ID of the image.</param>
        /// <param name="useSSL">Whether to use HTTPS.</param>
        /// <returns>The fully qualified URL to the transformed image.</returns>
        public String ToURL(Url transformation, String uniqueIdentifier, Boolean useSSL = true)
        {
            return transformation.Secure(useSSL).BuildUrl(uniqueIdentifier);
        }

        /// <summary>
        /// Builds an HTML img tag for an image with the specified transformation.
        /// </summary>
        /// <param name="transformation">The URL builder with transformations.</param>
        /// <param name="uniqueIdentifier">The public ID of the image.</param>
        /// <param name="parameters">Optional HTML attributes for the img tag.</param>
        /// <param name="useSSL">Whether to use HTTPS.</param>
        /// <returns>An HTML img tag string.</returns>
        public String ToImageTag(Url transformation, String uniqueIdentifier, StringDictionary? parameters = null, Boolean useSSL = true)
        {
            return transformation.Secure(useSSL).BuildImageTag(uniqueIdentifier, parameters);
        }

        /// <summary>
        /// Builds a URL for an image without transformations.
        /// </summary>
        /// <param name="uniqueIdentifier">The public ID of the image.</param>
        /// <param name="useSSL">Whether to use HTTPS.</param>
        /// <returns>The fully qualified URL to the image.</returns>
        public String ToURL(String uniqueIdentifier, Boolean useSSL = true)
        {
            return Client.Api.UrlImgUp.Secure(useSSL).BuildUrl(uniqueIdentifier);
        }

        /// <summary>
        /// Builds an HTML img tag for an image without transformations.
        /// </summary>
        /// <param name="uniqueIdentifier">The public ID of the image.</param>
        /// <param name="parameters">Optional HTML attributes for the img tag.</param>
        /// <param name="useSSL">Whether to use HTTPS.</param>
        /// <returns>An HTML img tag string.</returns>
        public String ToImageTag(String uniqueIdentifier, StringDictionary? parameters = null, Boolean useSSL = true)
        {
            return Client.Api.UrlImgUp.Secure(useSSL).BuildImageTag(uniqueIdentifier, parameters);
        }

        /// <summary>
        /// Builds a URL for an image identified by GUID with the specified transformation.
        /// </summary>
        /// <param name="transformation">The URL builder with transformations.</param>
        /// <param name="uniqueIdentifier">The GUID of the image.</param>
        /// <param name="useSSL">Whether to use HTTPS.</param>
        /// <returns>The fully qualified URL to the transformed image.</returns>
        public String ToURL(Url transformation, Guid uniqueIdentifier, Boolean useSSL = true)
        {
            return ToURL(transformation, ToCloudinaryUID(uniqueIdentifier), useSSL);
        }

        /// <summary>
        /// Builds an HTML img tag for an image identified by GUID with the specified transformation.
        /// </summary>
        /// <param name="transformation">The URL builder with transformations.</param>
        /// <param name="uniqueIdentifier">The GUID of the image.</param>
        /// <param name="parameters">Optional HTML attributes for the img tag.</param>
        /// <param name="useSSL">Whether to use HTTPS.</param>
        /// <returns>An HTML img tag string.</returns>
        public String ToImageTag(Url transformation, Guid uniqueIdentifier, StringDictionary? parameters = null, Boolean useSSL = true)
        {
            return ToImageTag(transformation, ToCloudinaryUID(uniqueIdentifier), parameters, useSSL);
        }

        /// <summary>
        /// Builds a URL for an image identified by GUID without transformations.
        /// </summary>
        /// <param name="uniqueIdentifier">The GUID of the image.</param>
        /// <param name="useSSL">Whether to use HTTPS.</param>
        /// <returns>The fully qualified URL to the image.</returns>
        public String ToURL(Guid uniqueIdentifier, Boolean useSSL = true)
        {
            return ToURL(ToCloudinaryUID(uniqueIdentifier), useSSL);
        }

        /// <summary>
        /// Builds an HTML img tag for an image identified by GUID without transformations.
        /// </summary>
        /// <param name="uniqueIdentifier">The GUID of the image.</param>
        /// <param name="parameters">Optional HTML attributes for the img tag.</param>
        /// <param name="useSSL">Whether to use HTTPS.</param>
        /// <returns>An HTML img tag string.</returns>
        public String ToImageTag(Guid uniqueIdentifier, StringDictionary? parameters = null, Boolean useSSL = true)
        {
            return ToImageTag(ToCloudinaryUID(uniqueIdentifier), parameters, useSSL);
        }

        /// <summary>
        /// Creates a StringDictionary with an alt attribute for use in image tags.
        /// </summary>
        /// <param name="name">The alt text value.</param>
        /// <returns>A StringDictionary containing the alt attribute.</returns>
        public static StringDictionary GetImageTagParameters(String name)
        {
            var parameters = new StringDictionary();
            parameters.Add(new KeyValuePair<String, String>("alt", name));

            return parameters;
        }
    }
}
