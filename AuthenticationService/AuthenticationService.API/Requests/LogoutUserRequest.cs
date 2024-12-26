namespace AuthenticationService.API.Requests
{
    public class LogoutUserRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
