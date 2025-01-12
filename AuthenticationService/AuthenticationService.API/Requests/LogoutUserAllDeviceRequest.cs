using System.Text.Json.Serialization;

namespace AuthenticationService.API.Requests
{
    public class LogoutUserAllDeviceRequest : ValidatableRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
        [JsonExtensionData]
        public Dictionary<string, object>? AdditionalData { get; set; }

        protected override void Validate()
        {
            ValidateProperties();
        }
    }
}
