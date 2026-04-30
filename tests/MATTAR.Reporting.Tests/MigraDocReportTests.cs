using MATTAR.Reporting;
using Shouldly;

namespace MATTAR.Reporting.Tests;

public class MigraDocReportTests
{
    private readonly IReport _report = new MigraDocReport();
    private readonly string _templatePath = Path.Combine("Fixtures", "INVOICE.ddl");

    // Minimal 1×1 white PNG (valid, cross-platform)
    private static readonly byte[] MinimalPng = Convert.FromBase64String(
        "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAAC0lEQVQI12NgAAIABQAABjE+ibYAAAAASUVORK5CYII=");

    [Fact]
    public void GenerateReport_Throws_WhenTemplateNotFound()
    {
        var ex = Assert.Throws<FileNotFoundException>(() =>
            _report.GenerateReport("nonexistent.ddl", "out.pdf", new Dictionary<string, string?>()));

        ex.FileName.ShouldBe("nonexistent.ddl");
        ex.Message.ShouldContain("nonexistent.ddl");
    }

    [Fact]
    public void GenerateReport_ProducesPdfOutput()
    {
        var outputPath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.pdf");
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
            new FileInfo(outputPath).Length.ShouldBeGreaterThan(0);
        }
        finally
        {
            if (File.Exists(outputPath)) File.Delete(outputPath);
        }
    }

    [Fact]
    public void GenerateReport_HandlesNullValue_AsEmpty()
    {
        var outputPath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.pdf");
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
    public void GenerateReport_WithNullTables_DoesNotThrow()
    {
        var outputPath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.pdf");
        try
        {
            var result = _report.GenerateReport(
                _templatePath,
                outputPath,
                new Dictionary<string, string?>(),
                ownerPassword: null,
                tables: null);

            result.ShouldBe(outputPath);
            File.Exists(outputPath).ShouldBeTrue();
        }
        finally
        {
            if (File.Exists(outputPath)) File.Delete(outputPath);
        }
    }
}
