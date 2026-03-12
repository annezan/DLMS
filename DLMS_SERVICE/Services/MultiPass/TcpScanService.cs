using System.Net.Sockets;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace DLMS_SERVICE.Services.MultiPass;

public record TcpScanResult(string Key, bool Reachable, long Ms);

public interface ITcpScanService
{
    Task<List<TcpScanResult>> ParallelTcpScanAsync(
        IEnumerable<(string Ip, string Port)> targets, TimeSpan timeout);

    Task<TcpScanResult> SingleTcpScanAsync(string ip, string port, TimeSpan timeout);
}

public class TcpScanService : ITcpScanService
{
    private readonly ILogger<TcpScanService> _logger;

    public TcpScanService(ILogger<TcpScanService> logger)
    {
        _logger = logger;
    }

    public async Task<List<TcpScanResult>> ParallelTcpScanAsync(
        IEnumerable<(string Ip, string Port)> targets, TimeSpan timeout)
    {
        var targetList = targets.ToList();
        int scanned = 0;

        var tasks = targetList.Select(t => Task.Run(async () =>
        {
            var key = $"{t.Ip}:{t.Port}";
            try
            {
                using var client = new TcpClient();
                using var cts = new CancellationTokenSource(timeout);
                var sw = Stopwatch.StartNew();
                await client.ConnectAsync(t.Ip, int.Parse(t.Port), cts.Token);
                sw.Stop();
                var num = Interlocked.Increment(ref scanned);
                _logger.LogDebug("[Pre-scan {Num}/{Total}] {Key} : OK ({Ms}ms)",
                    num, targetList.Count, key, sw.ElapsedMilliseconds);
                return new TcpScanResult(key, true, sw.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                var num = Interlocked.Increment(ref scanned);
                _logger.LogDebug("[Pre-scan {Num}/{Total}] {Key} : ECHEC ({Error})",
                    num, targetList.Count, key, ex.Message);
                return new TcpScanResult(key, false, (long)timeout.TotalMilliseconds);
            }
        }));

        return (await Task.WhenAll(tasks)).ToList();
    }

    public async Task<TcpScanResult> SingleTcpScanAsync(string ip, string port, TimeSpan timeout)
    {
        var key = $"{ip}:{port}";
        try
        {
            using var client = new TcpClient();
            using var cts = new CancellationTokenSource(timeout);
            var sw = Stopwatch.StartNew();
            await client.ConnectAsync(ip, int.Parse(port), cts.Token);
            sw.Stop();
            _logger.LogDebug("[retry] {Key} : OK ({Ms}ms)", key, sw.ElapsedMilliseconds);
            return new TcpScanResult(key, true, sw.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            _logger.LogDebug("[retry] {Key} : ECHEC ({Error})", key, ex.Message);
            return new TcpScanResult(key, false, (long)timeout.TotalMilliseconds);
        }
    }
}
