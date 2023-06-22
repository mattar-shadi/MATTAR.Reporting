using MATTAR.Reporting;
Console.WriteLine("Example on how to use MATTAR.Reporting to populate a template document.");
Console.WriteLine("");
IReport report = new PdfReport();
string templateDocPath = "FACTURE.pdf";
string outputFilePath = $"Output\\{DateTime.Now:yyyy-MM-dd}FACTURE.pdf";
string path = report.GenerateReport(templateDocPath, outputFilePath,
    new Dictionary<string, string?>
    {
        { "Number","F2231323" },
        { "CustomerNumber","C12354" },
        { "JourneyNumber","J2231323" },
        { "EntryNumber","E31FF323" },
        { "Date", DateTime.Now.ToShortDateString() },
        { "Name", "MATTAR SASU" },
        { "Line1", "50 Avenue Pierre-George Latécoère" },
        { "ZipCode", "31520" },
        { "City", "RAMONVILLE-SAINT-AGNE (FRANCE)" },
        { "InvoiceLine1Description", "Stockage des marchandises (jour)" },
        { "InvoiceLine1UnitPrice", "45 000" },
        { "InvoiceLine1Qty", "2" },
        { "InvoiceLine1Total", "90 000" },
        { "TotalBeforeTax", "90 000" },
        { "TaxAmount", "17 100" },
        { "Total", "107 000" }
    }
);
Console.WriteLine("Output Directory : " + Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path));
