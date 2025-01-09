using System.Text.Json.Serialization;

namespace AuthenticationService.API.Requests
{
    public class LogoutUserAllDeviceRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
        [JsonExtensionData]
        public Dictionary<string, object>? AdditionalData { get; set; }
    }
}
