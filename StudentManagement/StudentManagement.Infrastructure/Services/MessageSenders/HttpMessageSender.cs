using Microsoft.Extensions.Logging;
using StudentManagement.Application.Commons.Interfaces.Services;
using StudentManagement.Domain.Entities;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace StudentManagement.Infrastructure.Services.MessageSenders
{
    public class HttpMessageSender : IMessageSender
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HttpMessageSender> _logger;

        public HttpMessageSender(IHttpClientFactory httpClientFactory, ILogger<HttpMessageSender> logger)
        {
            _httpClient = httpClientFactory.CreateClient("InboxApiClient"); // Use a named client
            _logger = logger;
        }

        public async Task<bool> SendAsync(OutboxMessage message)
        {
            var requestBody = new
            {
                messageId = message.Id,
                type = message.Type.ToString(),
                dateCreated = message.CreatedAt,
                payload = message.Payload
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("api/inboxmessages", content); // Base URL comes from named client
                
                var responseBody = await response.Content.ReadAsStringAsync(); // Read response body as a string

                _logger.LogInformation(
                    $"Message with id {message.Id} sent to Inbox API. Status code: {response.StatusCode}. Response body: {responseBody}"
                );

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send message with id {message.Id}.");
                return false;
            }
        }
    }
}
