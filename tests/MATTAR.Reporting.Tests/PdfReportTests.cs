using MATTAR.Reporting;
using Shouldly;

namespace MATTAR.Reporting.Tests;

public class PdfReportTests
{
    private readonly IReport _report = new PdfReport();
    private readonly string _templatePath = Path.Combine("Fixtures", "FACTURE.pdf");

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
}
