using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static System.Console;
namespace SimpleAPI.Infrastructure.Service;

public sealed class RedisWarmupService(IServiceProvider _serviceProvider) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var cache = scope.ServiceProvider.GetRequiredService<IDistributedCache>();

        try
        {
            await cache.SetStringAsync("warmup-key", "ok", cancellationToken);
            var test = await cache.GetStringAsync("warmup-key", cancellationToken);
            WriteLine(test == "ok" ? "Redis warmup successful" : "Redis warmup failed");
        }
        catch (Exception ex)
        {
            WriteLine($"Redis warmup error: {ex.Message}");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}

