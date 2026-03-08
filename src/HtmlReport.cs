using Scriban;

namespace MATTAR.Reporting
{
    public class HtmlReport : IReport
    {
        public string GenerateReport(
            string templateDocPath,
            string outputPathFile,
            Dictionary<string, string?> datas,
            string ownerPassword = "MATTAR.Reporting")
        {
            // 1. Read the HTML template
            string templateContent = File.ReadAllText(templateDocPath);

            // 2. Replace placeholders via Scriban
            var template = Template.Parse(templateContent);
            if (template.HasErrors)
            {
                throw new InvalidOperationException(
                    $"Scriban template error(s): {string.Join(", ", template.Messages)}");
            }
            var scriptObject = new Scriban.Runtime.ScriptObject();
            foreach (var data in datas)
            {
                // Use SetValue with readOnly: false to disable Scriban's automatic snake_case renaming
                scriptObject.SetValue(data.Key, data.Value, readOnly: false);
            }
            var context = new Scriban.TemplateContext();
            context.PushGlobal(scriptObject);
            string renderedHtml = template.Render(context);

            // 3. Create the output folder if needed
            string? folder = Path.GetDirectoryName(outputPathFile);
            if (folder != null && !Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            // 4. Save according to extension
            string extension = Path.GetExtension(outputPathFile).ToLowerInvariant();

            if (extension == ".html")
            {
                // ownerPassword is not applicable for HTML output and is intentionally ignored here
                File.WriteAllText(outputPathFile, renderedHtml);
            }
            else if (extension == ".pdf")
            {
                var document = TheArtOfDev.HtmlRenderer.PdfSharp.PdfGenerator.GeneratePdf(
                    renderedHtml,
                    PdfSharp.PageSize.A4);
                document.SecuritySettings.OwnerPassword = ownerPassword;
                document.SecuritySettings.PermitFullQualityPrint = true;
                document.SecuritySettings.PermitPrint = true;
                document.SecuritySettings.PermitAccessibilityExtractContent = false;
                document.SecuritySettings.PermitAnnotations = false;
                document.SecuritySettings.PermitAssembleDocument = false;
                document.SecuritySettings.PermitExtractContent = false;
                document.SecuritySettings.PermitFormsFill = false;
                document.SecuritySettings.PermitModifyDocument = false;
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
