using System;
using System.Collections.Generic;
using System.Text;

namespace TMS.Shared.Model.Auth
{
    public class Login
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class LoginDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class TokenResponseDto
    {
        public string Token { get; set; } = string.Empty;
    }
}
