using ClassManagement.Application.Common.Interfaces.Services;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ClassManagement.Infrastructure.Services.MessageSenders
{
    public class HttpMessageSender : IMessageSender
    {
        private readonly HttpClient _httpClient;

        public HttpMessageSender(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("AckApiClient"); // Use a named client
        }

        public async Task AckMessageReceivedAsync(Guid messageId, CancellationToken cancellationToken)
        {
            var url = $"https://localhost:7202/api/outboxmessages/{messageId}";
            var response = await _httpClient.PostAsync(url, null, cancellationToken);

            // Ensure the status code is successful
            response.EnsureSuccessStatusCode();

            // Log the response status code and body
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
            Console.WriteLine($"Message with id {messageId} acknowledged. " +
                              $"Status Code: {response.StatusCode}, Response Body: {responseBody}");
        }
    }
}
