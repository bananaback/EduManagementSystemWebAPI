namespace AuthenticationService.API.Requests
{
    public class RotateTokenRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
