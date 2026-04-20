using PdfSharpCore.Drawing;
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
            string? ownerPassword = "MATTAR.Reporting",
            Dictionary<string, string?>? images = null,
            Dictionary<string, IEnumerable<Dictionary<string, string?>>>? tables = null)
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

            if (images != null)
            {
                foreach (var image in images)
                {
                    if (image.Value == null)
                        continue;

                    if (!File.Exists(image.Value))
                        throw new FileNotFoundException($"Image file not found: '{image.Value}'", image.Value);

                    if (!form.Fields.Names.Contains(image.Key))
                        continue;

                    var field = form.Fields[image.Key];
                    if (field is not PdfPushButtonField)
                        continue;

                    // Find the page and rectangle by resolving the field's /P and /Rect elements
                    var pageObj = field.Elements.GetObject("/P");
                    PdfPage? fieldPage = null;
                    for (int i = 0; i < template.Pages.Count; i++)
                    {
                        if (ReferenceEquals(template.Pages[i], pageObj))
                        {
                            fieldPage = template.Pages[i];
                            break;
                        }
                    }

                    if (fieldPage == null)
                        continue;

                    var fieldRect = field.Elements.GetRectangle("/Rect");

                    using var gfx = XGraphics.FromPdfPage(fieldPage);
                    using var xImage = XImage.FromFile(image.Value);

                    // PDF coordinates have origin at bottom-left; XGraphics origin is top-left
                    double x = fieldRect.X1;
                    double y = fieldPage.Height.Point - fieldRect.Y2;
                    double w = fieldRect.X2 - fieldRect.X1;
                    double h = fieldRect.Y2 - fieldRect.Y1;

                    gfx.DrawImage(xImage, x, y, w, h);
                }
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
