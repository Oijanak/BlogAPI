using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application.Options
{
    public class JwtSettings
    {
        public const string Key = "JwtSettings";

        public string Issuer { get; set; } = default!;
        public string Audience { get; set; } = default!;
        public string SecretKey { get; set; } = default!;
        public int ExpirationMinutes { get; set; }
    }
}
