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

        public FixerIOClient(String sAPIKey) :
            this (new FixerIOConfiguration(sAPIKey))
        {
            
        }

        public FixerIOClient(FixerIOConfiguration oConfiguration)
        {
            Configuration = oConfiguration;

            Validate();

            var oClientOptions = new RestClientOptions(Configuration.URL)
            {                
                Timeout = TimeSpan.FromMilliseconds(Configuration.Timeout > 0 ? Configuration.Timeout : 3 * 1000),
                ThrowOnAnyError = true,
                AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate,
                UserAgent = GetUserAgent(),
                Proxy = Configuration.Proxy
            };

            Client = new RestClient(oClientOptions,
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
            return String.Format("{0} - v{1}", Assembly.GetEntryAssembly().GetName().Name, Assembly.GetEntryAssembly().GetName().Version.ToString());
        }

        public void Dispose()
        {
            Client.Dispose();
        }

        protected async Task<T> ExceptionHandlerAsync<T>(Func<Task<T>> oAction)
        {
            try
            {
                return await oAction();
            }
            catch (Exception ex)
            {
                throw new FixerIOClientException($"API Exception: '{ex.Message}'", ex);
            }
        }

        protected async Task<T> Get<T>(String sResource, params Parameter[] oParameters)
        {
            return await ExceptionHandlerAsync(async () =>
            {
                var oRequest = new RestRequest(sResource);
                oRequest.AddParameter("access_key", Configuration.APIKey);

                foreach (var oParameter in oParameters)
                    oRequest.AddParameter(oParameter);

                return await Client.GetAsync<T>(oRequest);
            });
        }

        public async Task<FixerIODataSymbols> GetSymbols()
        {
            return await Get<FixerIODataSymbols>("symbols");
        }

        public async Task<FixerIODataRates> GetLatestRates()
        {
            return await Get<FixerIODataRates>("latest");
        }

        public async Task<FixerIODataRates> GetLatestRates(String sBaseCurrency)
        {
            return await Get<FixerIODataRates>("latest", Parameter.CreateParameter("base", sBaseCurrency, ParameterType.GetOrPost));
        }

        public async Task<FixerIODataRates> GetHistoricalRates(DateTime dtDate, String sBaseCurrency = null)
        {
            if (sBaseCurrency == null)
                return await Get<FixerIODataRates>($"{dtDate:yyyy-MM-dd}");

            return await Get<FixerIODataRates>($"{dtDate:yyyy-MM-dd}", Parameter.CreateParameter("base", sBaseCurrency, ParameterType.GetOrPost));
        }
    }
}

//API Documentation: https://fixer.io/documentation
