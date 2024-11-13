using System.Diagnostics.Metrics;

namespace QuickLinker.API.Observability;

public class QuickLinkerDiagnostic : IQuickLinkerDiagnostic
{
    public const string MeterName = nameof(QuickLinkerDiagnostic);
    public const string CreateShortLink = $"{MeterName}.CreateShortLink";
    public const string GetOriginalLink = $"{MeterName}.GetOriginalLink";

    private readonly Counter<long> _createShortLinkCounter;
    private readonly Counter<long> _getOriginalLinkCounter;

    public QuickLinkerDiagnostic(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create(MeterName);

        _createShortLinkCounter = meter.CreateCounter<long>(CreateShortLink);
        _getOriginalLinkCounter = meter.CreateCounter<long>(GetOriginalLink);
    }

    public void AddShortLink() => _createShortLinkCounter.Add(1);

    public void AddOriginalLink() => _getOriginalLinkCounter.Add(1);
}
