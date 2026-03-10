using Scriban;

namespace MATTAR.Reporting
{
    public class HtmlReport : IReport
    {
        public string GenerateReport(
            string templateDocPath,
            string outputPathFile,
            Dictionary<string, string?> datas,
            string? ownerPassword = "MATTAR.Reporting")
        {
            if (!File.Exists(templateDocPath))
                throw new FileNotFoundException($"HTML template not found: '{templateDocPath}'", templateDocPath);

            // 1. Read the HTML template
            string templateContent = File.ReadAllText(templateDocPath);

            // 2. Replace placeholders via Scriban
            var template = Template.Parse(templateContent);
            if (template.HasErrors)
                throw new InvalidOperationException(
                    $"Template parsing failed: {string.Join("; ", template.Messages)}");
            var scriptObject = new Scriban.Runtime.ScriptObject();
            foreach (var data in datas)
            {
                scriptObject.Add(data.Key, data.Value);
            }
            var context = new Scriban.TemplateContext();
            context.PushGlobal(scriptObject);
            string renderedHtml = template.Render(context);

            // 3. Create the output folder if needed
            string? folder = Path.GetDirectoryName(outputPathFile);
            if (!string.IsNullOrEmpty(folder) && !Directory.Exists(folder))
            {
                try
                {
                    Directory.CreateDirectory(folder);
                }
                catch (Exception ex)
                {
                    throw new DirectoryNotFoundException($"Output directory could not be created: '{folder}'", ex);
                }
            }

            // 4. Save according to extension
            string extension = Path.GetExtension(outputPathFile).ToLowerInvariant();

            if (extension == ".html")
            {
                File.WriteAllText(outputPathFile, renderedHtml);
            }
            else if (extension == ".pdf")
            {
                var document = TheArtOfDev.HtmlRenderer.PdfSharp.PdfGenerator.GeneratePdf(
                    renderedHtml,
                    PdfSharp.PageSize.A4);

                if (!string.IsNullOrEmpty(ownerPassword))
                {
                    document.SecuritySettings.OwnerPassword = ownerPassword;
                    document.SecuritySettings.PermitFullQualityPrint = true;
                    document.SecuritySettings.PermitPrint = true;
                    document.SecuritySettings.PermitAccessibilityExtractContent = false;
                    document.SecuritySettings.PermitAnnotations = false;
                    document.SecuritySettings.PermitAssembleDocument = false;
                    document.SecuritySettings.PermitExtractContent = false;
                    document.SecuritySettings.PermitFormsFill = false;
                    document.SecuritySettings.PermitModifyDocument = false;
                }

                document.Save(outputPathFile);
            }
            else
            {
                throw new NotSupportedException($"Unsupported output format: {extension}");
            }

            return outputPathFile;
        }
    }
}
