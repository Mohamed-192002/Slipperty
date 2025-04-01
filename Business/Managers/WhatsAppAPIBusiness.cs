using Microsoft.Extensions.Options;
using RestSharp;
using System.Net.Http;

namespace Business.Managers
{
    public interface IWhatsAppAPIBusiness
    {
        Task<int> SendOtpToWhatsApp(string otpCode, string phoneNumber);
        Task<int> SendOrdersExcelToLoginUserWhatsApp(byte[] fileContent, string phoneNumber);

    }
    public class WhatsAppAPIBusiness : IWhatsAppAPIBusiness
    {
        private readonly HttpClient _httpClient;
        private readonly string _url;
        private readonly string _instanceId;
        private readonly string _token;

        public WhatsAppAPIBusiness(HttpClient httpClient, IOptions<WhatsappAPISetting> apiSettings)
        {
            _url = apiSettings.Value.APIURL;
            _instanceId = apiSettings.Value.InstanceID;
            _token = apiSettings.Value.Token;
            _httpClient = httpClient;
        }

        public async Task<int> SendOtpToWhatsApp(string otpCode, string phoneNumber)
        {
            try
            {
               
                var url = $"https://api.ultramsg.com/instance{_instanceId}/messages/chat";  

               
                var client = new RestClient(url);
                var request = new RestRequest { Method = Method.Post };  

               
                request.AddHeader("content-type", "application/x-www-form-urlencoded");

               
                request.AddParameter("token", _token);
                request.AddParameter("to", $"+{phoneNumber}");  
                request.AddParameter("body", $"رمز التفعيل الخاص بك هو : {otpCode}");  

               
                RestResponse response = await client.ExecuteAsync(request);

               
                if (response.IsSuccessful)
                {
                   
                    Console.WriteLine("OTP sent successfully.");
                    return 1;  // Success
                }
                else
                {
                   
                    Console.WriteLine("Failed to send OTP: " + response.Content);
                    return 0;  // Failure
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return 0;  // Failure
            }
        }
        
        public async Task<int> SendOrdersExcelToLoginUserWhatsApp(byte[] fileContent, string phoneNumber)
        {
            try
            {
                var url = $"https://api.ultramsg.com/instance{_instanceId}/messages/chat";  
                var client = new RestClient(url);
                var request = new RestRequest { Method = Method.Post };
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                var tempFilePath = Path.Combine(Path.GetTempPath(), "Orders.xlsx");
                await File.WriteAllBytesAsync(tempFilePath, fileContent);  
                request.AddParameter("token", _token);
                request.AddParameter("to", $"+{phoneNumber}");
                request.AddParameter("caption", "Here is your requested Excel file");
                request.AddFile("document", tempFilePath);  
                RestResponse response = await client.ExecuteAsync(request);
                File.Delete(tempFilePath);
                if (response.IsSuccessful)
                {
                    Console.WriteLine("Excel file sent successfully.");
                    return 1;  
                }
                else
                {
                    Console.WriteLine("Failed to send Excel file: " + response.Content);
                    return 0;  
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return 0;  
            }
        }

    }
}
