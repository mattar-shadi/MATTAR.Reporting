using MATTAR.Reporting;
using Shouldly;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata.Profiles.Icc;
using SixLabors.ImageSharp.PixelFormats;

namespace MATTAR.Reporting.Tests;

public class PdfReportTests
{
    private readonly IReport _report = new PdfReport();
    private readonly string _templatePath = Path.Combine("Fixtures", "FACTURE.pdf");

    // Minimal 1×1 white PNG (valid, cross-platform)
    private static readonly byte[] MinimalPng = Convert.FromBase64String(
        "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAAC0lEQVQI12NgAAIABQAABjE+ibYAAAAASUVORK5CYII=");

    [Fact]
    public void GenerateReport_Throws_WhenTemplateNotFound()
    {
        var ex = Assert.Throws<FileNotFoundException>(() =>
            _report.GenerateReport("nonexistent.pdf", "out.pdf", new Dictionary<string, string?>()));

        ex.FileName.ShouldBe("nonexistent.pdf");
        ex.Message.ShouldContain("nonexistent.pdf");
    }

    [Fact]
    public void GenerateReport_SkipsUnknownFields_WhenFieldNotInForm()
    {
        var outputPath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.pdf");
        try
        {
            var result = _report.GenerateReport(
                _templatePath,
                outputPath,
                new Dictionary<string, string?> { { "NonExistentField_XYZ", "value" } },
                ownerPassword: null);

            result.ShouldBe(outputPath);
            File.Exists(outputPath).ShouldBeTrue();
        }
        finally
        {
            if (File.Exists(outputPath)) File.Delete(outputPath);
        }
    }

    [Fact]
    public void GenerateReport_HandlesNullValue_AsEmptyString()
    {
        var outputPath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.pdf");
        try
        {
            var result = _report.GenerateReport(
                _templatePath,
                outputPath,
                new Dictionary<string, string?> { { "Number", null } },
                ownerPassword: null);

            result.ShouldBe(outputPath);
            File.Exists(outputPath).ShouldBeTrue();
        }
        finally
        {
            if (File.Exists(outputPath)) File.Delete(outputPath);
        }
    }

    [Fact]
    public void GenerateReport_WithEmptyPassword_DoesNotApplySecurity()
    {
        var outputPath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.pdf");
        try
        {
            var result = _report.GenerateReport(
                _templatePath,
                outputPath,
                new Dictionary<string, string?>(),
                ownerPassword: "");

            result.ShouldBe(outputPath);
            File.Exists(outputPath).ShouldBeTrue();
        }
        finally
        {
            if (File.Exists(outputPath)) File.Delete(outputPath);
        }
    }

    [Fact]
    public void GenerateReport_WithNullPassword_DoesNotApplySecurity()
    {
        var outputPath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.pdf");
        try
        {
            var result = _report.GenerateReport(
                _templatePath,
                outputPath,
                new Dictionary<string, string?>(),
                ownerPassword: null);

            result.ShouldBe(outputPath);
            File.Exists(outputPath).ShouldBeTrue();
        }
        finally
        {
            if (File.Exists(outputPath)) File.Delete(outputPath);
        }
    }

    [Fact]
    public void GenerateReport_CreatesOutputDirectory_WhenMissing()
    {
        var tempBase = Path.Combine(Path.GetTempPath(), $"reporting_test_{Guid.NewGuid()}");
        var outputPath = Path.Combine(tempBase, "subdir", "output.pdf");
        try
        {
            var result = _report.GenerateReport(
                _templatePath,
                outputPath,
                new Dictionary<string, string?>(),
                ownerPassword: null);

            result.ShouldBe(outputPath);
            File.Exists(outputPath).ShouldBeTrue();
        }
        finally
        {
            if (Directory.Exists(tempBase)) Directory.Delete(tempBase, true);
        }
    }

    [Fact]
    public void GenerateReport_Throws_WhenOutputDirectoryCannotBeCreated()
    {
        // Use a file as a parent "directory" — Directory.CreateDirectory will fail
        var tempFile = Path.GetTempFileName();
        try
        {
            var outputPath = Path.Combine(tempFile, "output.pdf");

            Assert.Throws<DirectoryNotFoundException>(() =>
                _report.GenerateReport(
                    _templatePath,
                    outputPath,
                    new Dictionary<string, string?>(),
                    ownerPassword: null));
        }
        finally
        {
            if (File.Exists(tempFile)) File.Delete(tempFile);
        }
    }

    [Fact]
    public void GenerateReport_WithImages_Throws_WhenImageFileNotFound()
    {
        var outputPath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.pdf");
        try
        {
            var ex = Assert.Throws<FileNotFoundException>(() =>
                _report.GenerateReport(
                    _templatePath,
                    outputPath,
                    new Dictionary<string, string?>(),
                    ownerPassword: null,
                    images: new Dictionary<string, string?> { { "Logo", "nonexistent_image.png" } }));

            ex.FileName.ShouldBe("nonexistent_image.png");
            ex.Message.ShouldContain("nonexistent_image.png");
        }
        finally
        {
            if (File.Exists(outputPath)) File.Delete(outputPath);
        }
    }

    [Fact]
    public void GenerateReport_WithImages_SkipsNullImageValues()
    {
        var outputPath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.pdf");
        try
        {
            var result = _report.GenerateReport(
                _templatePath,
                outputPath,
                new Dictionary<string, string?>(),
                ownerPassword: null,
                images: new Dictionary<string, string?> { { "Logo", null } });

            result.ShouldBe(outputPath);
            File.Exists(outputPath).ShouldBeTrue();
        }
        finally
        {
            if (File.Exists(outputPath)) File.Delete(outputPath);
        }
    }

    [Fact]
    public void GenerateReport_WithImages_SkipsSilently_WhenImageFieldNotInForm()
    {
        var imagePath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.png");
        var outputPath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.pdf");
        try
        {
            File.WriteAllBytes(imagePath, MinimalPng);

            var result = _report.GenerateReport(
                _templatePath,
                outputPath,
                new Dictionary<string, string?>(),
                ownerPassword: null,
                images: new Dictionary<string, string?> { { "NonExistentImageField_XYZ", imagePath } });

            result.ShouldBe(outputPath);
            File.Exists(outputPath).ShouldBeTrue();
        }
        finally
        {
            if (File.Exists(imagePath)) File.Delete(imagePath);
            if (File.Exists(outputPath)) File.Delete(outputPath);
        }
    }

    [Theory]
    [InlineData("png", false)]
    [InlineData("jpg", false)]
    [InlineData("png", true)]
    public void GenerateReport_WithImages_EmbedsImageInPdf(string extension, bool withColorProfile)
    {
        var tempDirectory = Path.Combine(Path.GetTempPath(), $"reporting_test_{Guid.NewGuid()}");
        Directory.CreateDirectory(tempDirectory);
        try
        {
            var imagePath = Path.Combine(tempDirectory, $"logo.{extension}");
            var templatePath = Path.Combine(tempDirectory, "template.pdf");
            var outputPath = Path.Combine(tempDirectory, "output.pdf");
            using (var image = new Image<Rgba32>(2, 2, new Rgba32(255, 255, 255, 128)))
            {
                if (withColorProfile)
                    image.Metadata.IccProfile = new IccProfile();
                image.Save(imagePath);
            }
            if (withColorProfile)
            {
                using var savedImage = Image.Load(imagePath);
                savedImage.Metadata.IccProfile.ShouldNotBeNull();
            }

            using (var template = PdfReader.Open(_templatePath, PdfDocumentOpenMode.Modify))
            {
                var page = template.Pages[0];
                var field = template.AcroForm.Fields["Number"];
                field.Elements.SetName("/FT", "/Btn");
                field.Elements.SetInteger("/Ff", 65536); // Push button
                field.Elements["/P"] = page.Reference;
                template.Save(templatePath);
            }

            _report.GenerateReport(
                templatePath,
                outputPath,
                new Dictionary<string, string?>(),
                ownerPassword: null,
                images: new Dictionary<string, string?> { { "Number", imagePath } })
                .ShouldBe(outputPath);

            File.ReadAllText(outputPath).ShouldContain("/Subtype /Image");
        }
        finally
        {
            Directory.Delete(tempDirectory, true);
        }
    }
}
