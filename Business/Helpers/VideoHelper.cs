using System.Net.Http;
using System.Threading.Tasks;

namespace Business.Helpers
{
    public static class VideoHelper
    {
        public static string GetYouTubeVideoId(string url)
        {
            var regex = new System.Text.RegularExpressions.Regex(@"(?:https?:\/\/(?:www\.)?youtube\.com\/(?:[^\/\n\s]+\/\S+\/|(?:v|e(?:mbed)?)\/|\S*\?v=))([^""&?=]+)");
            var match = regex.Match(url);
            return match.Success ? match.Groups[1].Value : string.Empty;
        }

        public static string GetTikTokVideoId(string url)
        {
            // Extract the video ID from the TikTok URL
            var regex = new System.Text.RegularExpressions.Regex(@"\/video\/(\d+)");
            var match = regex.Match(url);
            return match.Success ? match.Groups[1].Value : string.Empty;
        }


        public static async Task<string> GetTikTokVideoIdAsync(string url)
        {
            // Step 1: Resolve the actual URL from the redirect link
            using (var httpClient = new HttpClient())
            {
                try
                {
                    // Follow the redirect to get the full URL
                    var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                    var finalUrl = response.Headers.Location?.ToString() ?? url;

                    // Step 2: Extract the video ID from the resolved URL
                    var regex = new System.Text.RegularExpressions.Regex(@"\/video\/(\d+)");
                    var match = regex.Match(finalUrl);
                    return match.Success ? match.Groups[1].Value : string.Empty;
                }
                catch
                {
                    // Handle exceptions (e.g., network errors)
                    return string.Empty;
                }
            }
        }


    }


}
