using RestSharp;
using RestSharp.Serializers.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BAUERGROUP.Shared.Cloud.FixerIO
{
    /// <summary>
    /// Client for accessing the Fixer.io foreign exchange rates API.
    /// </summary>
    /// <remarks>
    /// Provides methods to retrieve currency symbols, latest exchange rates, and historical rates.
    /// API documentation: https://fixer.io/documentation
    /// </remarks>
    public class FixerIOClient : IDisposable
    {
        /// <summary>
        /// Gets the configuration settings for the Fixer.io client.
        /// </summary>
        protected FixerIOConfiguration Configuration { get; private set; }

        /// <summary>
        /// Gets the underlying REST client instance.
        /// </summary>
        protected RestClient Client { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FixerIOClient"/> class with the specified API key.
        /// </summary>
        /// <param name="apiKey">The Fixer.io API key.</param>
        public FixerIOClient(String apiKey) :
            this (new FixerIOConfiguration(apiKey))
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FixerIOClient"/> class with the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration containing API key and connection settings.</param>
        /// <exception cref="FixerIOClientException">Thrown when the API URL is invalid.</exception>
        public FixerIOClient(FixerIOConfiguration configuration)
        {
            Configuration = configuration;

            Validate();

            var clientOptions = new RestClientOptions(Configuration.URL)
            {
                Timeout = TimeSpan.FromMilliseconds(Configuration.Timeout > 0 ? Configuration.Timeout : 3 * 1000),
                ThrowOnAnyError = true,
                AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate,
                UserAgent = GetUserAgent(),
                Proxy = Configuration.Proxy
            };

            Client = new RestClient(clientOptions,
                configureSerialization: s => s.UseSystemTextJson(new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                    WriteIndented = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.Never,
                    Converters = { new JsonStringEnumConverter() }
                })
            );

            Initialize();
        }

        private void Validate()
        {
            if (!Uri.IsWellFormedUriString(Configuration.URL, UriKind.Absolute))
                throw new FixerIOClientException($"Invalid API URL '{Configuration.URL}'.", new ArgumentException("Invalid URL"));
        }

        private void Initialize()
        {
            Client.AddDefaultHeader(KnownHeaders.Accept, "application/json");
        }

        private static string GetUserAgent()
        {
            var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
            var version = assembly.GetName().Version?.ToString() ?? "1.0.0";
            var name = assembly.GetName().Name ?? "Unknown";
            return $"{name} - v{version}";
        }

        /// <summary>
        /// Releases all resources used by the <see cref="FixerIOClient"/>.
        /// </summary>
        public void Dispose()
        {
            Client.Dispose();
            GC.SuppressFinalize(this);
        }

        protected async Task<T> ExceptionHandlerAsync<T>(Func<Task<T>> action)
        {
            try
            {
                return await action();
            }
            catch (Exception ex)
            {
                throw new FixerIOClientException($"API Exception: '{ex.Message}'", ex);
            }
        }

        protected async Task<T?> Get<T>(String resource, params Parameter[] parameters)
        {
            return await ExceptionHandlerAsync(async () =>
            {
                var request = new RestRequest(resource);
                request.AddParameter("access_key", Configuration.APIKey);

                foreach (var parameter in parameters)
                    request.AddParameter(parameter);

                return await Client.GetAsync<T>(request);
            });
        }

        /// <summary>
        /// Retrieves all available currency symbols and their names.
        /// </summary>
        /// <returns>A data object containing a dictionary of currency codes and names.</returns>
        /// <exception cref="FixerIOClientException">Thrown when the API request fails.</exception>
        public async Task<FixerIODataSymbols?> GetSymbols()
        {
            return await Get<FixerIODataSymbols>("symbols");
        }

        /// <summary>
        /// Retrieves the latest exchange rates with EUR as the base currency.
        /// </summary>
        /// <returns>A data object containing current exchange rates.</returns>
        /// <exception cref="FixerIOClientException">Thrown when the API request fails.</exception>
        public async Task<FixerIODataRates?> GetLatestRates()
        {
            return await Get<FixerIODataRates>("latest");
        }

        /// <summary>
        /// Retrieves the latest exchange rates for a specific base currency.
        /// </summary>
        /// <param name="baseCurrency">The base currency code (e.g., "USD", "GBP").</param>
        /// <returns>A data object containing current exchange rates relative to the base currency.</returns>
        /// <exception cref="FixerIOClientException">Thrown when the API request fails.</exception>
        public async Task<FixerIODataRates?> GetLatestRates(String baseCurrency)
        {
            return await Get<FixerIODataRates>("latest", Parameter.CreateParameter("base", baseCurrency, ParameterType.GetOrPost));
        }

        /// <summary>
        /// Retrieves historical exchange rates for a specific date.
        /// </summary>
        /// <param name="date">The date for which to retrieve exchange rates.</param>
        /// <param name="baseCurrency">Optional base currency code. Defaults to EUR if not specified.</param>
        /// <returns>A data object containing historical exchange rates for the specified date.</returns>
        /// <exception cref="FixerIOClientException">Thrown when the API request fails.</exception>
        public async Task<FixerIODataRates?> GetHistoricalRates(DateTime date, String? baseCurrency = null)
        {
            if (baseCurrency == null)
                return await Get<FixerIODataRates>($"{date:yyyy-MM-dd}");

            return await Get<FixerIODataRates>($"{date:yyyy-MM-dd}", Parameter.CreateParameter("base", baseCurrency, ParameterType.GetOrPost));
        }
    }
}

//API Documentation: https://fixer.io/documentation
