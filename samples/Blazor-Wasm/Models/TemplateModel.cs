namespace MattarReportBlazor.Models
{
    /// <summary>
    /// Représente un template de document.
    /// </summary>
    public class TemplateModel
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public TemplateType Type { get; set; }
        public Dictionary<string, string> SampleData { get; set; } = new();
        public Dictionary<string, List<Dictionary<string, string>>> SampleTables { get; set; } = new();
    }

    public enum TemplateType
    {
        DDL,  // MigraDoc DDL (pour PDF)
        HTML  // HTML (pour HTML/PDF)
    }
}
