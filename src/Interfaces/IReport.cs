namespace MATTAR.Reporting
{
    public interface IReport
    {
        string GenerateReport(
            string templateDocPath,
            string outputPathFile,
            Dictionary<string, string?> datas,
            string? ownerPassword = "MATTAR.Reporting",
            Dictionary<string, string?>? images = null);
    }

    public interface ITableReport : IReport
    {
        string GenerateReport(
            string templateDocPath,
            string outputPathFile,
            Dictionary<string, string?> datas,
            string? ownerPassword = "MATTAR.Reporting",
            Dictionary<string, string?>? images = null,
            Dictionary<string, IEnumerable<Dictionary<string, string?>>>? tables = null);
    }
}