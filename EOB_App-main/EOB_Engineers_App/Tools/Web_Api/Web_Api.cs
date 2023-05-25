using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Utilities.Web_Api
{
    public abstract class Web_Api
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _client;
        private readonly ILogger<Web_Api> _logger;

        public Web_Api(IConfiguration config, ILogger<Web_Api> logger)
        {
            _config = config;
            _logger = logger;

#if DEBUG
            // turn off ssl validation during debug
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            _client = new HttpClient(httpClientHandler);
#else
            _client = new HttpClient();
#endif

            _client.DefaultRequestHeaders.Accept.Clear();
        }

        public HttpClient Client => _client;

        protected virtual void Initialize_Client(string key_Name)
        {
            string api = _config.GetValue<string>(key_Name);

            _client.BaseAddress = new Uri(api);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
