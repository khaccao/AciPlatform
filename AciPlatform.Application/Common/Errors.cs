using System;

namespace AciPlatform.Application.Common
{
    public class Errors
    {
        public const string NotFound = "Resource not found.";
        public const string InvalidData = "Invalid input data.";
        public const string ServerError = "Internal server error.";
    }

    public class AppSettings
    {
        public string ConnectionString { get; set; }
        public string JwtSecret { get; set; }
    }
}
