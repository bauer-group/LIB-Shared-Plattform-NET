using BAUERGROUP.Shared.Core.Application;
using BAUERGROUP.Shared.Core.Extensions;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serializers.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BAUERGROUP.Shared.API.API
{
    /// <summary>
    /// A generic REST API client that provides common HTTP operations (GET, POST, PUT, DELETE) with JSON serialization.
    /// </summary>
    /// <remarks>
    /// This client uses RestSharp internally and supports automatic JSON serialization/deserialization,
    /// gzip/deflate compression, and configurable timeouts and proxy settings.
    /// </remarks>
    public class GenericAPIClient : IDisposable
    {
        /// <summary>
        /// Gets the underlying RestSharp client instance.
        /// </summary>
        protected RestClient Client { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericAPIClient"/> class.
        /// </summary>
        /// <param name="url">The base URL of the API endpoint.</param>
        /// <param name="timeout">The request timeout in milliseconds. Defaults to 10000 (10 seconds).</param>
        /// <param name="proxy">Optional web proxy to use for requests.</param>
        /// <exception cref="GenericAPIClientException">Thrown when the URL is invalid.</exception>
        public GenericAPIClient(String url, Int32 timeout = 10 * 1000, IWebProxy? proxy = null)
        {
            Validate(url);

            var clientOptions = new RestClientOptions(url)
            {
                Timeout = TimeSpan.FromMilliseconds(timeout > 0 ? timeout : 10 * 1000),
                ThrowOnAnyError = true,
                AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate,
                UserAgent = GetUserAgent(),
                Proxy = proxy,
                //PreAuthenticate = true,
            };

            Validate(clientOptions);

            Client = new RestClient(clientOptions,
                configureSerialization: s => s.UseSystemTextJson(new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                    WriteIndented = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.Never,
                    Converters = { new JsonStringEnumConverter() }
                })
            );

            Initalize();
            Authenticate();
        }

        private void Initalize()
        {
            Client.AddDefaultHeader(KnownHeaders.Accept, "application/json");
        }

        private void Authenticate()
        {
            //Client.Authenticator = new HttpBasicAuthenticator("username", "password");
            //Client.Authenticator = OAuth1Authenticator.ForRequestToken(consumerKey, consumerSecret);
            //Client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(
            //    "Bearer", token
            //);
            //Client.Authenticator = new JwtAuthenticator(myToken);
        }

        private static string GetUserAgent()
        {
            return String.Format("{0} - v{1}", ApplicationProperties.Name ?? "App", ApplicationProperties.Version?.ToString() ?? "1.0");
        }

        private void Validate(String url)
        {
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                throw new GenericAPIClientException($"Invalid API URL '{url}'.", new ArgumentException("Invalid URL"));
        }

        private void Validate(RestClientOptions clientOptions)
        {

        }

        /// <summary>
        /// Releases all resources used by the <see cref="GenericAPIClient"/>.
        /// </summary>
        public void Dispose()
        {
            Client?.Dispose();
        }

        protected async Task<T> ExceptionHandlerAsync<T>(Func<Task<T>> action)
        {
            try
            {
                return await action();
            }
            catch (Exception ex)
            {
                throw new GenericAPIClientException($"API Exception: '{ex.Message}'", ex);
            }
        }

        /// <summary>
        /// Performs an asynchronous GET request and deserializes the response to the specified type.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the response to.</typeparam>
        /// <param name="resource">The resource path relative to the base URL.</param>
        /// <returns>The deserialized response object, or null if the response is empty.</returns>
        /// <exception cref="GenericAPIClientException">Thrown when the request fails.</exception>
        public async Task<T?> Get<T>(String resource)
        {
            return await ExceptionHandlerAsync(async () =>
            {
                var request = new RestRequest(resource);
                return await Client.GetAsync<T>(request);
            });
        }

        /// <summary>
        /// Performs an asynchronous GET request without expecting a response body.
        /// </summary>
        /// <param name="resource">The resource path relative to the base URL.</param>
        /// <exception cref="GenericAPIClientException">Thrown when the request fails.</exception>
        public async Task Get(String resource)
        {
            await ExceptionHandlerAsync(async () =>
            {
                var request = new RestRequest(resource);
                return await Client.GetAsync(request);
            });
        }

        /// <summary>
        /// Performs an asynchronous POST request with a JSON body and deserializes the response.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the response to.</typeparam>
        /// <typeparam name="U">The type of the request body.</typeparam>
        /// <param name="resource">The resource path relative to the base URL.</param>
        /// <param name="data">The data to send as JSON in the request body.</param>
        /// <returns>The deserialized response object, or null if the response is empty.</returns>
        /// <exception cref="GenericAPIClientException">Thrown when the request fails.</exception>
        public async Task<T?> Post<T, U>(String resource, U data) where U : class
        {
            return await ExceptionHandlerAsync(async () =>
            {
                var request = new RestRequest(resource).AddJsonBody(data);
                return await Client.PostAsync<T>(request);
            });
        }

        /// <summary>
        /// Performs an asynchronous POST request with a JSON body without expecting a response body.
        /// </summary>
        /// <typeparam name="U">The type of the request body.</typeparam>
        /// <param name="resource">The resource path relative to the base URL.</param>
        /// <param name="data">The data to send as JSON in the request body.</param>
        /// <exception cref="GenericAPIClientException">Thrown when the request fails.</exception>
        public async Task Post<U>(String resource, U data) where U : class
        {
            await ExceptionHandlerAsync(async () =>
            {
                var request = new RestRequest(resource).AddJsonBody(data);
                return await Client.PostAsync(request);
            });
        }

        /// <summary>
        /// Performs an asynchronous PUT request with a JSON body and deserializes the response.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the response to.</typeparam>
        /// <typeparam name="U">The type of the request body.</typeparam>
        /// <param name="resource">The resource path relative to the base URL.</param>
        /// <param name="data">The data to send as JSON in the request body.</param>
        /// <returns>The deserialized response object, or null if the response is empty.</returns>
        /// <exception cref="GenericAPIClientException">Thrown when the request fails.</exception>
        public async Task<T?> Put<T, U>(String resource, U data) where U : class
        {
            return await ExceptionHandlerAsync(async () =>
            {
                var request = new RestRequest(resource).AddJsonBody(data);
                return await Client.PutAsync<T>(request);
            });
        }

        /// <summary>
        /// Performs an asynchronous PUT request with a JSON body without expecting a response body.
        /// </summary>
        /// <typeparam name="U">The type of the request body.</typeparam>
        /// <param name="resource">The resource path relative to the base URL.</param>
        /// <param name="data">The data to send as JSON in the request body.</param>
        /// <exception cref="GenericAPIClientException">Thrown when the request fails.</exception>
        public async Task Put<U>(String resource, U data) where U : class
        {
            await ExceptionHandlerAsync(async () =>
            {
                var request = new RestRequest(resource).AddJsonBody(data);
                return await Client.PutAsync(request);
            });
        }

        /// <summary>
        /// Performs an asynchronous DELETE request and deserializes the response.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the response to.</typeparam>
        /// <param name="resource">The resource path relative to the base URL.</param>
        /// <returns>The deserialized response object, or null if the response is empty.</returns>
        /// <exception cref="GenericAPIClientException">Thrown when the request fails.</exception>
        public async Task<T?> Delete<T>(String resource)
        {
            return await ExceptionHandlerAsync(async () =>
            {
                var request = new RestRequest(resource);
                return await Client.DeleteAsync<T>(request);
            });
        }

        /// <summary>
        /// Performs an asynchronous DELETE request without expecting a response body.
        /// </summary>
        /// <param name="resource">The resource path relative to the base URL.</param>
        /// <exception cref="GenericAPIClientException">Thrown when the request fails.</exception>
        public async Task Delete(String resource)
        {
            await ExceptionHandlerAsync(async () =>
            {
                var request = new RestRequest(resource);
                return await Client.DeleteAsync(request);
            });
        }

        /// <summary>
        /// Downloads binary data from the specified resource.
        /// </summary>
        /// <param name="resource">The resource path relative to the base URL.</param>
        /// <returns>The downloaded data as a byte array, or null if the download fails.</returns>
        /// <exception cref="GenericAPIClientException">Thrown when the request fails.</exception>
        public async Task<Byte[]?> Download(String resource)
        {
            return await ExceptionHandlerAsync(async () =>
            {
                var request = new RestRequest(resource);
                return await Client.DownloadDataAsync(request);
            });
        }

        /// <summary>
        /// Processes URL parameters from the data object and replaces placeholders in the resource path.
        /// </summary>
        /// <typeparam name="T">The type of the data object containing URL parameters.</typeparam>
        /// <param name="resource">The resource path with optional placeholders.</param>
        /// <param name="data">The data object containing values for URL parameters.</param>
        /// <returns>The resource path with placeholders replaced by actual values.</returns>
        public String ResourceParameterProcessor<T>(String resource, T? data) where T : class
        {
            //Without Data
            if (data == null)
                return resource;

            //With Data
            return data.SetURLParameters(resource);
        }

        /// <summary>
        /// Executes an API call with the specified HTTP method without expecting a typed response.
        /// </summary>
        /// <typeparam name="U">The type of the request body.</typeparam>
        /// <param name="resource">The resource path relative to the base URL.</param>
        /// <param name="method">The HTTP method to use.</param>
        /// <param name="data">Optional data to send with the request.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="NotImplementedException">Thrown when an unsupported HTTP method is specified.</exception>
        public Task Call<U>(String resource, GenericAPIClientMethod method, U? data = null) where U : class
        {
            switch (method)
            {
                case GenericAPIClientMethod.GET:
                    return Get(ResourceParameterProcessor(resource, data));

                case GenericAPIClientMethod.POST:
                    return Post(resource, data!);

                case GenericAPIClientMethod.PUT:
                    return Put(resource, data!);

                case GenericAPIClientMethod.DELETE:
                    return Delete(ResourceParameterProcessor(resource, data));

                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Executes an API call with the specified HTTP method and deserializes the response.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the response to.</typeparam>
        /// <typeparam name="U">The type of the request body.</typeparam>
        /// <param name="resource">The resource path relative to the base URL.</param>
        /// <param name="method">The HTTP method to use.</param>
        /// <param name="data">Optional data to send with the request.</param>
        /// <returns>The deserialized response object, or null if the response is empty.</returns>
        /// <exception cref="NotImplementedException">Thrown when an unsupported HTTP method is specified.</exception>
        public Task<T?> Call<T, U>(String resource, GenericAPIClientMethod method, U? data = null) where U : class
        {
            switch (method)
            {
                case GenericAPIClientMethod.GET:
                    return Get<T>(ResourceParameterProcessor(resource, data));

                case GenericAPIClientMethod.POST:
                    return Post<T, U>(resource, data!);

                case GenericAPIClientMethod.PUT:
                    return Put<T, U>(resource, data!);

                case GenericAPIClientMethod.DELETE:
                    return Delete<T>(ResourceParameterProcessor(resource, data));

                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Executes an API call and returns the raw response content as a string.
        /// </summary>
        /// <typeparam name="U">The type of the request body.</typeparam>
        /// <param name="resource">The resource path relative to the base URL.</param>
        /// <param name="method">The HTTP method to use.</param>
        /// <param name="data">Optional data to send with the request.</param>
        /// <returns>The raw response content as a string, or null if the response is empty.</returns>
        /// <exception cref="GenericAPIClientException">Thrown when the request fails.</exception>
        /// <exception cref="NotImplementedException">Thrown when an unsupported HTTP method is specified.</exception>
        public async Task<String?> Execute<U>(String resource, GenericAPIClientMethod method, U? data = null) where U : class
        {
            return await ExceptionHandlerAsync(async () =>
            {
                var url = (method == GenericAPIClientMethod.GET || method == GenericAPIClientMethod.DELETE) ? ResourceParameterProcessor(resource, data) : resource;
                var request = new RestRequest(url);

                if ((method == GenericAPIClientMethod.POST || method == GenericAPIClientMethod.PUT) && data != null)
                    request.AddJsonBody(data);

                var restMethod = Method.Options;
                switch (method)
                {
                    case GenericAPIClientMethod.GET:
                        restMethod = Method.Get;
                        break;

                    case GenericAPIClientMethod.POST:
                        restMethod = Method.Post;
                        break;

                    case GenericAPIClientMethod.PUT:
                        restMethod = Method.Put;
                        break;

                    case GenericAPIClientMethod.DELETE:
                        restMethod = Method.Delete;
                        break;

                    default:
                        throw new NotImplementedException();
                }

                var result = await Client.ExecuteAsync(request, restMethod);
                return result.Content;
            });
        }
    }
}
