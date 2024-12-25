﻿namespace AuthenticationService.API.Requests
{
    public class RegisterUserRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword {  get; set; } = string.Empty;
    }
}
