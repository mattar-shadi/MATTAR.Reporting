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
            string ownerPassword = "MATTAR.Reporting")
        {
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
                
                ((PdfTextField)form.Fields[data.Key]).Value = new PdfString(data.Value);
            }

            template.SecuritySettings.OwnerPassword = ownerPassword;
            template.SecuritySettings.PermitFullQualityPrint = true;
            template.SecuritySettings.PermitPrint = true;
            template.SecuritySettings.PermitAccessibilityExtractContent = false;
            template.SecuritySettings.PermitAnnotations = false;
            template.SecuritySettings.PermitAssembleDocument = false;
            template.SecuritySettings.PermitExtractContent = false;
            template.SecuritySettings.PermitFormsFill = false;
            template.SecuritySettings.PermitModifyDocument = false;

            string? folder = Path.GetDirectoryName(outputPath);
            if (folder != null && !Directory.Exists(folder))
            {
                DirectoryInfo directoryInfo = Directory.CreateDirectory(folder);
            }

            template.Save(outputPath);

            return outputPath;
        }
    }
}
