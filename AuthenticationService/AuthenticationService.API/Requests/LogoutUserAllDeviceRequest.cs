namespace AuthenticationService.API.Requests
{
    public class LogoutUserAllDeviceRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
