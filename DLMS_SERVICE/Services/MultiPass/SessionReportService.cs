using System.Text;
using Microsoft.Extensions.Logging;

namespace DLMS_SERVICE.Services.MultiPass;

public interface ISessionReportService
{
    void LogSessionReport(ReadSessionReport report);
}

public class SessionReportService : ISessionReportService
{
    private readonly ILogger<SessionReportService> _logger;

    public SessionReportService(ILogger<SessionReportService> logger)
    {
        _logger = logger;
    }

    public void LogSessionReport(ReadSessionReport report)
    {
        var sb = new StringBuilder();
        sb.AppendLine();
        sb.AppendLine($"=== RAPPORT SESSION #{report.SessionNumber} (Cycle #{report.CycleId}) ===");
        sb.AppendLine($"Date: {DateTime.Now:yyyy-MM-dd HH:mm} — Duree: {report.TotalElapsedMs / 60000.0:F0}min");
        sb.AppendLine($"Compteurs: {report.TotalMetersInScope} en scope, {report.TotalSucceeded} lus ({report.TauxReussite:F1}%), {report.TotalFailed} non-lus");
        sb.AppendLine();

        // Per-pass details
        foreach (var pass in report.Passes)
        {
            var durationMin = pass.ElapsedMs / 60000.0;
            var throughput = durationMin > 0 ? pass.Succeeded / durationMin : 0;
            var rate = pass.Results.Count > 0 ? (double)pass.Succeeded / pass.Results.Count * 100 : 0;

            sb.AppendLine($"--- Passe {pass.PassNumber} (budget {pass.ElapsedMs / 1000}s) ---");
            sb.AppendLine($"  Compteurs: {pass.InScope} -> {pass.Succeeded} lus, {pass.Failed} echoues, {pass.DeferredCount} differes");
            sb.AppendLine($"  IPs differees: {pass.DeferredIps.Count}");
            sb.AppendLine($"  Debit: {throughput:F1} compteurs/min");
            sb.AppendLine();
        }

        // Pass summary table
        sb.AppendLine($"  {"Pass",-8} {"Duree",-10} {"In Scope",-10} {"OK",-6} {"Taux",-8} {"Debit",-12}");
        sb.AppendLine($"  {new string('-', 54)}");

        foreach (var pass in report.Passes)
        {
            var durationMin = pass.ElapsedMs / 60000.0;
            var throughput = durationMin > 0 ? pass.Succeeded / durationMin : 0;
            var rate = pass.Results.Count > 0 ? (double)pass.Succeeded / pass.Results.Count * 100 : 0;

            sb.AppendLine($"  Pass {pass.PassNumber,-3} {durationMin,-9:F1}m {pass.InScope,-10} {pass.Succeeded,-6} {rate,-7:F1}% {throughput,-11:F1}/min");
        }

        var totalMin = report.TotalReadingMs / 60000.0;
        var totalThroughput = totalMin > 0 ? report.TotalSucceeded / totalMin : 0;
        sb.AppendLine($"  {new string('-', 54)}");
        sb.AppendLine($"  {"TOTAL",-8} {totalMin,-9:F1}m {report.TotalMetersInScope,-10} {report.TotalSucceeded,-6} {report.TauxReussite,-7:F1}% {totalThroughput,-11:F1}/min");
        sb.AppendLine($"  {"Pauses",-8} +{report.TotalPauseMs / 60000.0:F1}m");
        sb.AppendLine();

        // Unread breakdown by reason
        if (report.UnreadByReason.Count > 0)
        {
            sb.AppendLine($"--- Non-lus par motif ---");
            foreach (var kv in report.UnreadByReason.OrderByDescending(kv => kv.Value.Count))
            {
                var pct = report.TotalMetersInScope > 0
                    ? (double)kv.Value.Count / report.TotalMetersInScope * 100 : 0;
                sb.AppendLine($"  {kv.Key,-30} : {kv.Value.Count,4} ({pct:F1}%)");

                // Show up to 5 IP examples for IP-based categories
                if (kv.Key.Contains("IP") || kv.Key.Contains("Concentrateur"))
                {
                    var ipExamples = report.AllResults
                        .Where(r => kv.Value.Contains(r.Serial))
                        .Select(r => r.Ip)
                        .Where(ip => !string.IsNullOrEmpty(ip))
                        .Distinct()
                        .Take(5);
                    if (ipExamples.Any())
                    {
                        sb.AppendLine($"    IPs: {string.Join(", ", ipExamples)}");
                    }
                }
            }
            sb.AppendLine();
        }

        // Top 5 slowest IPs
        var ipStats = report.AllResults
            .Where(r => r.TotalMs > 0 && !string.IsNullOrEmpty(r.Ip))
            .GroupBy(r => r.Ip)
            .Select(g => new
            {
                Ip = g.Key,
                AvgMs = g.Average(r => (double)r.TotalMs),
                SuccessRate = g.Count() > 0 ? (double)g.Count(r => r.Success) / g.Count() * 100 : 0,
                Count = g.Count()
            })
            .OrderByDescending(s => s.AvgMs)
            .Take(5)
            .ToList();

        if (ipStats.Count > 0)
        {
            sb.AppendLine("--- Top 5 IPs les plus lentes ---");
            foreach (var s in ipStats)
            {
                sb.AppendLine($"  {s.Ip}: {s.AvgMs / 1000:F0}s avg ({s.SuccessRate:F0}% succes, {s.Count} compteurs)");
            }
            sb.AppendLine();
        }

        // Recommendations
        var recommendations = GenerateRecommendations(report);
        if (recommendations.Count > 0)
        {
            sb.AppendLine("--- Recommandations ---");
            foreach (var rec in recommendations)
            {
                sb.AppendLine($"  - {rec}");
            }
            sb.AppendLine();
        }

        _logger.LogInformation(sb.ToString());
    }

    private List<string> GenerateRecommendations(ReadSessionReport report)
    {
        var recommendations = new List<string>();

        // Dead IPs
        if (report.UnreadByReason.TryGetValue("IP morte", out var deadIpMeters))
        {
            var deadIps = report.AllResults
                .Where(r => deadIpMeters.Contains(r.Serial))
                .Select(r => r.Ip)
                .Distinct()
                .Count();
            recommendations.Add($"{deadIps} IPs mortes a verifier par l'equipe reseau");
        }

        // Unstable concentrators
        if (report.UnreadByReason.TryGetValue("Concentrateur instable", out var unstableMeters))
        {
            var unstableIps = report.AllResults
                .Where(r => unstableMeters.Contains(r.Serial))
                .Select(r => r.Ip)
                .Distinct()
                .Count();
            recommendations.Add($"{unstableIps} concentrateurs instables necessitant maintenance");
        }

        // Low success rate
        if (report.TauxReussite < 50)
        {
            recommendations.Add($"Taux de reussite faible ({report.TauxReussite:F1}%) — verifier la connectivite reseau globale");
        }

        // Missing keys
        if (report.UnreadByReason.TryGetValue("Cle manquante", out var missingKeyMeters))
        {
            recommendations.Add($"{missingKeyMeters.Count} compteurs avec cles manquantes a provisionner");
        }

        return recommendations;
    }
}
