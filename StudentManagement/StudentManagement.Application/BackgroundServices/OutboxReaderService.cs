using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StudentManagement.Application.Commons.Interfaces.Repositories;
using StudentManagement.Application.Commons.Interfaces.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StudentManagement.Application.BackgroundServices
{
    public class OutboxReaderService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<OutboxReaderService> _logger;

        public OutboxReaderService(IServiceScopeFactory serviceScopeFactory, ILogger<OutboxReaderService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var outboxRepository = scope.ServiceProvider.GetRequiredService<IOutboxRepository>();
                    var messageSender = scope.ServiceProvider.GetRequiredService<IMessageSender>();

                    var message = await outboxRepository.GetOldestUnprocessedAsync(stoppingToken);

                    if (message != null)
                    {
                        await messageSender.SendAsync(message);
                    }

                    _logger.LogInformation("OutboxReaderService is working.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred in OutboxReaderService.");
                }

                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }
        }
    }
}
