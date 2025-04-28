namespace Application.Interfaces;

public interface IRemediationService
{
    Task<string> BuildRemediationAsync(
        IReadOnlyList<string> weakTopics, CancellationToken ct);
}