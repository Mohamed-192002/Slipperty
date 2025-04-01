using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

namespace Business.Managers
{
    public interface IGoogleSheetBusiness
    {
        Task<bool> AppendDataAsync(IList<IList<object>> values);
    }
    public class GoogleSheetBusiness : IGoogleSheetBusiness
    {
        private readonly string _spreadsheetId = "15T8ORXRRMwFFM2ZiOGE_kMT6D0P84FhJ1pqfrElmOhE"; // Replace with your Google Sheet ID
        private readonly string _range = "Sheet1!A1"; // Adjust range as needed
        private readonly string _credentialsPath = "wwwroot//Credentials.json"; // Path to your service account credentials

        private SheetsService _sheetsService;

        public GoogleSheetBusiness()
        {
            InitializeGoogleSheetsService().Wait();
        }

        // Initialize the Google Sheets service using the service account credentials
        private async Task InitializeGoogleSheetsService()
        {
            try
            {
                // Authenticate using service account credentials
                var credential = GoogleCredential.FromFile(_credentialsPath)
                                                  .CreateScoped(SheetsService.Scope.Spreadsheets);

                // Create the Sheets API client
                _sheetsService = new SheetsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Google Sheets API App"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing Google Sheets Service: {ex.Message}");
                throw;
            }
        }

        // Append data to Google Sheet
        public async Task<bool> AppendDataAsync(IList<IList<object>> values)
        {
            try
            {
                var valueRange = new ValueRange
                {
                    Values = values
                };

                // Create the append request
                var appendRequest = _sheetsService.Spreadsheets.Values.Append(valueRange, _spreadsheetId, _range);
                appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.RAW;

                // Execute the append request
                var response = await appendRequest.ExecuteAsync();

                // Check if the response has a valid SpreadsheetId (indicating successful append)
                if (!string.IsNullOrEmpty(response?.SpreadsheetId))
                {
                    return true;  // Data was appended successfully
                }

                return false;  // No data appended or an issue occurred
            }
            catch (Exception ex)
            {
                // Handle any errors, e.g., logging
                Console.WriteLine($"Error appending data: {ex.Message}");
                return false;
            }
        }


    }
}
