namespace AuthenticationService.API.Requests
{
    public class LoginUserRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
