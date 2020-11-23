namespace ESFA.DC.FRM.ReportService.Interfaces
{
    public interface ISummaryRow
    {
        string Report { get; }

        string Title { get; }

        int NumberOfQueries { get; }
    }
}
