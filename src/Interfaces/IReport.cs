namespace MATTAR.Reporting
{
    public interface IReport
    {
        string GenerateReport(
            string templateDocPath, 
            string outputPathFile, 
            Dictionary<string, string?> datas, 
            string ownerPassword = "MATTAR.Reporting");
    }
}