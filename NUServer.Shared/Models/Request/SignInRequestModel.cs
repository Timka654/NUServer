﻿using System.ComponentModel.DataAnnotations;

namespace NUServer.Shared.Models.Request
{
    public class SignInRequestModel
    {
        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
