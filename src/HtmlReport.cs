using Scriban;
using Scriban.Runtime;

namespace MATTAR.Reporting
{
    public class HtmlReport : IReport
    {
        public string GenerateReport(
            string templateDocPath,
            string outputPathFile,
            Dictionary<string, string?> datas,
            string? ownerPassword = "MATTAR.Reporting",
            Dictionary<string, string?>? images = null,
            Dictionary<string, IEnumerable<Dictionary<string, string?>>>? tables = null)
        {
            if (!File.Exists(templateDocPath))
                throw new FileNotFoundException($"HTML template not found: '{templateDocPath}'", templateDocPath);

            // 1. Read the HTML template
            string templateContent = File.ReadAllText(templateDocPath);

            // 2. Replace placeholders via Scriban
            var template = Template.Parse(templateContent);
            var scriptObject = new ScriptObject();
            foreach (var data in datas)
            {
                scriptObject.Add(data.Key, data.Value);
            }

            if (images != null)
            {
                foreach (var image in images)
                {
                    if (image.Value == null)
                        continue;

                    if (!File.Exists(image.Value))
                        throw new FileNotFoundException($"Image file not found: '{image.Value}'", image.Value);

                    string mimeType = GetMimeType(image.Value);
                    byte[] bytes = File.ReadAllBytes(image.Value);
                    string base64 = Convert.ToBase64String(bytes);
                    scriptObject.Add(image.Key, $"data:{mimeType};base64,{base64}");
                }
            }

            if (tables != null)
            {
                foreach (var table in tables)
                {
                    var scriptArray = new ScriptArray();
                    foreach (var row in table.Value)
                    {
                        var rowObject = new ScriptObject();
                        foreach (var cell in row)
                        {
                            rowObject.Add(cell.Key, cell.Value);
                        }
                        scriptArray.Add(rowObject);
                    }
                    scriptObject.Add(table.Key, scriptArray);
                }
            }

            var context = new TemplateContext();
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
#if BROWSER
                throw new PlatformNotSupportedException(
                    "HTML-to-PDF rendering via HtmlRenderer is not supported on WebAssembly. " +
                    "Use MigraDocReport for PDF generation in Blazor WebAssembly.");
#else
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
#endif
            }
            else
            {
                throw new NotSupportedException($"Unsupported output format: {extension}");
            }

            return outputPathFile;
        }

        private static string GetMimeType(string filePath) =>
            Path.GetExtension(filePath).ToLowerInvariant() switch
            {
                ".png" => "image/png",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".gif" => "image/gif",
                ".webp" => "image/webp",
                ".bmp" => "image/bmp",
                _ => "application/octet-stream"
            };
    }
}
