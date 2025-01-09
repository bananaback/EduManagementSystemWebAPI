using AuthenticationService.API.Validators;
using System.Text.Json.Serialization;

namespace AuthenticationService.API.Requests
{
    public class LoginUserRequest : ValidatableRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        [JsonExtensionData]
        public Dictionary<string, object>? AdditionalData { get; set; }

        protected override void Validate()
        {
            ValidateProperties();
        }
    }
}
