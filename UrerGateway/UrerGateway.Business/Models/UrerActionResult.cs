using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Hosting;

namespace UrerGateway.Business.Models
{
    public class UrerActionResult : IHttpActionResult
    {
        public readonly object Data;
        private readonly HttpStatusCode statusCode;

        public UrerActionResult(HttpStatusCode statusCode, object data)
        {
            this.statusCode = statusCode;
            this.Data = data;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(this.CreateResponse());
        }

        public HttpResponseMessage CreateResponse()

        {
            var request = new HttpRequestMessage();
            request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var response = request.CreateResponse(this.statusCode, this.Data);

            return response;
        }
    }
}