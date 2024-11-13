namespace QuickLinker.API.Observability;

public interface IQuickLinkerDiagnostic
{
    void AddOriginalLink();
    void AddShortLink();
}