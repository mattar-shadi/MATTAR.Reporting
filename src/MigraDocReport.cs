using MigraDocCore.DocumentObjectModel.IO;
using MigraDocCore.Rendering;
using Scriban;
using Scriban.Runtime;

namespace MATTAR.Reporting
{
    /// <summary>
    /// Generates a PDF from a MigraDoc DDL template using Scriban for placeholder substitution.
    /// This implementation uses only MIT-licensed packages and is compatible with Blazor WebAssembly.
    /// </summary>
    public class MigraDocReport : IReport
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
                throw new FileNotFoundException($"MigraDoc DDL template not found: '{templateDocPath}'", templateDocPath);

            // 1. Read the DDL template and replace placeholders via Scriban
            string templateContent = File.ReadAllText(templateDocPath);
            var template = Template.Parse(templateContent);
            var scriptObject = new ScriptObject();

            foreach (var data in datas)
                scriptObject.Add(data.Key, data.Value);

            if (images != null)
            {
                foreach (var image in images)
                {
                    if (image.Value == null)
                        continue;

                    if (!File.Exists(image.Value))
                        throw new FileNotFoundException($"Image file not found: '{image.Value}'", image.Value);

                    scriptObject.Add(image.Key, image.Value);
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
                            rowObject.Add(cell.Key, cell.Value);
                        scriptArray.Add(rowObject);
                    }
                    scriptObject.Add(table.Key, scriptArray);
                }
            }

            var context = new TemplateContext();
            context.PushGlobal(scriptObject);
            string renderedDdl = template.Render(context);

            // 2. Create the output directory if needed
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

            // 3. Parse the rendered DDL into a MigraDoc Document
            var document = DdlReader.DocumentFromString(renderedDdl);

            // 4. Render to PDF
            var renderer = new PdfDocumentRenderer(unicode: true);
            renderer.Document = document;
            renderer.RenderDocument();

            if (!string.IsNullOrEmpty(ownerPassword))
            {
                renderer.PdfDocument.SecuritySettings.OwnerPassword = ownerPassword;
                renderer.PdfDocument.SecuritySettings.PermitFullQualityPrint = true;
                renderer.PdfDocument.SecuritySettings.PermitPrint = true;
                renderer.PdfDocument.SecuritySettings.PermitAccessibilityExtractContent = false;
                renderer.PdfDocument.SecuritySettings.PermitAnnotations = false;
                renderer.PdfDocument.SecuritySettings.PermitAssembleDocument = false;
                renderer.PdfDocument.SecuritySettings.PermitExtractContent = false;
                renderer.PdfDocument.SecuritySettings.PermitFormsFill = false;
                renderer.PdfDocument.SecuritySettings.PermitModifyDocument = false;
            }

            renderer.PdfDocument.Save(outputPathFile);

            return outputPathFile;
        }
    }
}
