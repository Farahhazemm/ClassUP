using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ClassUP.ApplicationCore.DTOs.Responses.Auth.Refresh
{
    public class TokensDTO
    {
         public string JwtToken {  get; set; }

         public   DateTime JwtTokenExpiresOn {  get; set; }

        [JsonIgnore]
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }

    }
}
