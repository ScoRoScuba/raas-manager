namespace OFX.RAASManager.Config
{
    public class AuthServerConfig
    {
        public string Url { get; set; }
        public bool AllowUntrustedCertificate { get; set; }
        public int JwtValidationClockSkewInSeconds { get; set; }
    }
}
