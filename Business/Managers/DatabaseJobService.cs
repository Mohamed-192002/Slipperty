using Business.Helpers;
using Castle.Core.Configuration;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
namespace Business.Managers
{
    public class StoredProcedureBackgroundService : BackgroundService
    {
        private readonly ILogger<StoredProcedureBackgroundService> _logger;
        private readonly string _connectionString;
        private readonly IServiceProvider _serviceProvider;

        public StoredProcedureBackgroundService(ILogger<StoredProcedureBackgroundService> logger
            , IServiceProvider serviceProvider)
        {
            _logger = logger;
            _connectionString = "Server=db16214.public.databaseasp.net; Database=db16214; User Id=db16214; Password=Wq2%?5Zd4Qn#; Encrypt=True; TrustServerCertificate=True; MultipleActiveResultSets=True;";
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    TimeSpan timeToMidnight = GetTimeUntilNextExecution();
                    _logger.LogInformation($"Service will wait {timeToMidnight} until midnight.");
                    // Wait until the next midnight (12 AM)
                    await Task.Delay(timeToMidnight, stoppingToken);
                    // Execute stored procedure
                    RunStoredProcedure();
                    // Resolve INotifyOrderTransactionService within the scope
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var notifyOrderTransactionService = scope.ServiceProvider.GetRequiredService<INotifyOrderTransactionService>();
                        notifyOrderTransactionService.SendOrderTransactionNotification();
                    }

                    _logger.LogInformation("Task successfully executed.");

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while executing stored procedure.");
                }
                // Wait for 24 hours before running again
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }


        private void RunStoredProcedure()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("ReturnHoldingOrdersToNewOrders_v2", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
            }
        }
        private TimeSpan GetTimeUntilNextExecution()
        {
            DateTime now = DateTime.Now;
            DateTime nextRun = DateTime.Today.AddDays(1).AddHours(0); // 12:00 AM the next day
            return nextRun - now;
        }
    }
}
