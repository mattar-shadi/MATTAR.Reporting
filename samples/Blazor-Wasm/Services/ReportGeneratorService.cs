using MATTAR.Reporting;
using Scriban;
using Scriban.Runtime;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MattarReportBlazor.Services
{
    /// <summary>
    /// Service pour générer des rapports HTML et PDF à partir de templates.
    /// Utilise MATTAR.Reporting 1.0.1 avec support complet WASM.
    /// </summary>
    public class ReportGeneratorService
    {
        private readonly IReport _reportGenerator;

        public ReportGeneratorService()
        {
            _reportGenerator = new MigraDocReport();
        }

        /// <summary>
        /// Génère un rapport PDF à partir d'un template DDL.
        /// </summary>
        public async Task<byte[]> GeneratePdfReportAsync(
            string templateContent,
            Dictionary<string, string?> data,
            Dictionary<string, IEnumerable<Dictionary<string, string?>>>? tables = null)
        {
            try
            {
                // Créer des fichiers temporaires
                var tempDir = Path.Combine(Path.GetTempPath(), "MattarReports");
                Directory.CreateDirectory(tempDir);

                var templatePath = Path.Combine(tempDir, $"template_{Guid.NewGuid()}.ddl");
                var outputPath = Path.Combine(tempDir, $"report_{Guid.NewGuid()}.pdf");

                // Écrire le template
                await File.WriteAllTextAsync(templatePath, templateContent);

                // Générer le rapport avec la nouvelle API
                _reportGenerator.GenerateReport(
                    templatePath,
                    outputPath,
                    data,
                    "MATTAR.Reporting",
                    null,
                    tables);

                // Lire le fichier généré
                var pdfBytes = await File.ReadAllBytesAsync(outputPath);

                // Nettoyer les fichiers temporaires
                File.Delete(templatePath);
                File.Delete(outputPath);

                return pdfBytes;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la génération du PDF: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Génère un rapport HTML à partir d'un template HTML avec Scriban.
        /// </summary>
        public async Task<string> GenerateHtmlReportAsync(
            string templateContent,
            Dictionary<string, string?> data,
            Dictionary<string, IEnumerable<Dictionary<string, string?>>>? tables = null)
        {
            try
            {
                // Parser le template Scriban
                var template = Template.Parse(templateContent);
                var scriptObject = new ScriptObject();

                // Ajouter les données simples
                foreach (var kvp in data)
                    scriptObject.Add(kvp.Key, kvp.Value);

                // Ajouter les tableaux
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

                // Rendre le template
                var context = new TemplateContext();
                context.PushGlobal(scriptObject);
                var html = template.Render(context);

                return await Task.FromResult(html);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la génération du HTML: {ex.Message}", ex);
            }
        }
    }
}
