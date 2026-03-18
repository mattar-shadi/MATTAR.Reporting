using MATTAR.Reporting;
using Shouldly;

namespace MATTAR.Reporting.Tests;

public class HtmlReportTests
{
    private readonly IReport _report = new HtmlReport();
    private readonly string _templatePath = Path.Combine("Fixtures", "TEMPLATE.html");

    // Minimal 1×1 white PNG (valid, cross-platform)
    private static readonly byte[] MinimalPng = Convert.FromBase64String(
        "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAAC0lEQVQI12NgAAIABQAABjE+ibYAAAAASUVORK5CYII=");

    [Fact]
    public void GenerateReport_Throws_WhenTemplateNotFound()
    {
        var ex = Assert.Throws<FileNotFoundException>(() =>
            _report.GenerateReport("nonexistent.html", "out.html", new Dictionary<string, string?>()));

        ex.FileName.ShouldBe("nonexistent.html");
        ex.Message.ShouldContain("nonexistent.html");
    }

    [Fact]
    public void GenerateReport_ProducesHtmlOutput_WhenExtensionIsHtml()
    {
        var outputPath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.html");
        try
        {
            var result = _report.GenerateReport(
                _templatePath,
                outputPath,
                new Dictionary<string, string?>
                {
                    { "Number", "INV-001" },
                    { "Name", "Test Client" },
                    { "Date", "2024-01-01" },
                    { "Total", "1000" }
                });

            result.ShouldBe(outputPath);
            File.Exists(outputPath).ShouldBeTrue();
            var content = File.ReadAllText(outputPath);
            content.ShouldContain("INV-001");
        }
        finally
        {
            if (File.Exists(outputPath)) File.Delete(outputPath);
        }
    }

    [Fact]
    public void GenerateReport_HandlesNullValue_AsEmpty()
    {
        var outputPath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.html");
        try
        {
            var result = _report.GenerateReport(
                _templatePath,
                outputPath,
                new Dictionary<string, string?> { { "Number", null } });

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
        var outputPath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.html");
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
    public void GenerateReport_Throws_WhenUnsupportedExtension()
    {
        var outputPath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.docx");
        Assert.Throws<NotSupportedException>(() =>
            _report.GenerateReport(_templatePath, outputPath, new Dictionary<string, string?>()));
    }

    [Fact]
    public void GenerateReport_CreatesOutputDirectory_WhenMissing()
    {
        var tempBase = Path.Combine(Path.GetTempPath(), $"reporting_test_{Guid.NewGuid()}");
        var outputPath = Path.Combine(tempBase, "subdir", "output.html");
        try
        {
            var result = _report.GenerateReport(
                _templatePath,
                outputPath,
                new Dictionary<string, string?>());

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
            var outputPath = Path.Combine(tempFile, "output.html");

            Assert.Throws<DirectoryNotFoundException>(() =>
                _report.GenerateReport(
                    _templatePath,
                    outputPath,
                    new Dictionary<string, string?>()));
        }
        finally
        {
            if (File.Exists(tempFile)) File.Delete(tempFile);
        }
    }

    [Fact]
    public void GenerateReport_WithImages_InjectsBase64DataUri_InHtmlOutput()
    {
        var imagePath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.png");
        var outputPath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.html");
        var templatePath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.html");
        try
        {
            File.WriteAllBytes(imagePath, MinimalPng);
            File.WriteAllText(templatePath, "<html><body><img src=\"{{ Logo }}\" /></body></html>");

            var result = _report.GenerateReport(
                templatePath,
                outputPath,
                new Dictionary<string, string?>(),
                ownerPassword: null,
                images: new Dictionary<string, string?> { { "Logo", imagePath } });

            result.ShouldBe(outputPath);
            var content = File.ReadAllText(outputPath);
            content.ShouldContain("data:image/png;base64,");
        }
        finally
        {
            if (File.Exists(imagePath)) File.Delete(imagePath);
            if (File.Exists(outputPath)) File.Delete(outputPath);
            if (File.Exists(templatePath)) File.Delete(templatePath);
        }
    }

    [Fact]
    public void GenerateReport_WithImages_Throws_WhenImageFileNotFound()
    {
        var outputPath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.html");
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
        var outputPath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.html");
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
    public void GenerateReport_WithImages_DetectsMimeType_ForJpeg()
    {
        var imagePath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.jpg");
        var outputPath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.html");
        var templatePath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.html");
        try
        {
            // Write PNG bytes with a .jpg extension to test MIME detection only
            File.WriteAllBytes(imagePath, MinimalPng);
            File.WriteAllText(templatePath, "<html><body><img src=\"{{ Photo }}\" /></body></html>");

            var result = _report.GenerateReport(
                templatePath,
                outputPath,
                new Dictionary<string, string?>(),
                ownerPassword: null,
                images: new Dictionary<string, string?> { { "Photo", imagePath } });

            var content = File.ReadAllText(outputPath);
            content.ShouldContain("data:image/jpeg;base64,");
        }
        finally
        {
            if (File.Exists(imagePath)) File.Delete(imagePath);
            if (File.Exists(outputPath)) File.Delete(outputPath);
            if (File.Exists(templatePath)) File.Delete(templatePath);
        }
    }
}
