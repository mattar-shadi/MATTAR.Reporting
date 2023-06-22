namespace MATTAR.Reporting
{
    public class HtmlReport : IReport
    {
        public string GenerateReport(
            string templatePath,
            string outputPath,
            Dictionary<string, string?> datas,
            string ownerPassword = "MATTAR.Reporting")
        {
            //TODO
            throw new NotImplementedException();
        }
    }
}