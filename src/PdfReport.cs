using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.AcroForms;
using PdfSharpCore.Pdf.IO;

namespace MATTAR.Reporting
{
    public class PdfReport : IReport
    {
        public string GenerateReport(
            string templatePath,
            string outputPath,
            Dictionary<string, string?> datas,
            string? ownerPassword = "MATTAR.Reporting")
        {
            if (!File.Exists(templatePath))
                throw new FileNotFoundException($"PDF template not found: '{templatePath}'", templatePath);

            string? folder = Path.GetDirectoryName(outputPath);
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

            PdfDocument template = PdfReader.Open(templatePath, PdfDocumentOpenMode.Modify);
            PdfAcroForm form = template.AcroForm;

            if (form.Elements.ContainsKey("/NeedAppearances"))
                form.Elements["/NeedAppearances"] = new PdfBoolean(true);
            else
                form.Elements.Add("/NeedAppeareances", new PdfBoolean(true));

            foreach (var data in datas)
            {
                if (!form.Fields.Names.Contains(data.Key))
                    continue;

                ((PdfTextField)form.Fields[data.Key]).Value = new PdfString(data.Value ?? string.Empty);
            }

            if (!string.IsNullOrEmpty(ownerPassword))
            {
                template.SecuritySettings.OwnerPassword = ownerPassword;
                template.SecuritySettings.PermitFullQualityPrint = true;
                template.SecuritySettings.PermitPrint = true;
                template.SecuritySettings.PermitAccessibilityExtractContent = false;
                template.SecuritySettings.PermitAnnotations = false;
                template.SecuritySettings.PermitAssembleDocument = false;
                template.SecuritySettings.PermitExtractContent = false;
                template.SecuritySettings.PermitFormsFill = false;
                template.SecuritySettings.PermitModifyDocument = false;
            }

            template.Save(outputPath);

            return outputPath;
        }
    }
}
