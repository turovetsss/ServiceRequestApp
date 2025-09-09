namespace Infrastructure.Storage;

public class MinioOptions
{
    public string Endpoint { get; set; } = string.Empty; 
    public bool WithSSL { get; set; }
    public string AccessKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public string PublicEndpoint { get; set; } = string.Empty;
}