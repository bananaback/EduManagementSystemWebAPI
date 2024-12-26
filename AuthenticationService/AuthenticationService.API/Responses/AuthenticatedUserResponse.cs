﻿namespace AuthenticationService.API.Responses
{
    public class AuthenticatedUserResponse
    {
        public string Message { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken {  get; set; } = string.Empty;   
    }
}
