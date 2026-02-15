using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ClassUP.ApplicationCore.DTOs.Responses.Auth.Login
{
    public class LoginResponseDTO
    {
        public string Token { get; set; } = null!;
       // public DateTime Expiration { get; set; }
        [JsonIgnore]
        public string RefreshToken { get; set; } = null!;
        public DateTime RefreshTokenExpiresAt { get; set; }
    }
}
