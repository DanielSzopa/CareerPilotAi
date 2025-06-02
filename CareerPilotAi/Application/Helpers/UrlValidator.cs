namespace CareerPilotAi.Application.Helpers;

internal static class UrlValidator
{
    internal static void ValidateUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            throw new ArgumentException("URL cannot be null or empty", nameof(url));
        }

        if (!Uri.TryCreate(url, UriKind.Absolute, out Uri? uriResult)
            || (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
        {
            throw new ArgumentException("Invalid URL format. URL must be a valid HTTP or HTTPS URL", nameof(url));
        }
    }
}
