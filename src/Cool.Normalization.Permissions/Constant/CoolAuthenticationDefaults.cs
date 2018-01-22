using System;
using System.Collections.Generic;
using System.Text;

namespace Cool.Normalization.Permissions
{
    public class CoolAuthenticationDefaults
    {
        public const string Authority =  "http://id.housecool.com";
        public const bool RequireHttpsMetadata = false;
        public const string ApiName = "default-api";
        public const string ApiSecret = "secret";
    }
}
