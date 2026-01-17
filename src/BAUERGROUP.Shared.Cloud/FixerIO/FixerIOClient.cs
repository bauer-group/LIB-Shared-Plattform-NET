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
    public class FixerIOClient : IDisposable
    {
        protected FixerIOConfiguration Configuration { get; private set; }
        protected RestClient Client { get; private set; }

        public FixerIOClient(String apiKey) :
            this (new FixerIOConfiguration(apiKey))
        {

        }

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

            Initalize();
        }

        private void Validate()
        {
            if (!Uri.IsWellFormedUriString(Configuration.URL, UriKind.Absolute))
                throw new FixerIOClientException($"Invalid API URL '{Configuration.URL}'.", new ArgumentException("Invalid URL"));
        }

        private void Initalize()
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

        public void Dispose()
        {
            Client.Dispose();
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

        public async Task<FixerIODataSymbols?> GetSymbols()
        {
            return await Get<FixerIODataSymbols>("symbols");
        }

        public async Task<FixerIODataRates?> GetLatestRates()
        {
            return await Get<FixerIODataRates>("latest");
        }

        public async Task<FixerIODataRates?> GetLatestRates(String baseCurrency)
        {
            return await Get<FixerIODataRates>("latest", Parameter.CreateParameter("base", baseCurrency, ParameterType.GetOrPost));
        }

        public async Task<FixerIODataRates?> GetHistoricalRates(DateTime date, String? baseCurrency = null)
        {
            if (baseCurrency == null)
                return await Get<FixerIODataRates>($"{date:yyyy-MM-dd}");

            return await Get<FixerIODataRates>($"{date:yyyy-MM-dd}", Parameter.CreateParameter("base", baseCurrency, ParameterType.GetOrPost));
        }
    }
}

//API Documentation: https://fixer.io/documentation
