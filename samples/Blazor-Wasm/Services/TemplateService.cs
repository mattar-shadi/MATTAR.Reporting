using MattarReportBlazor.Models;

namespace MattarReportBlazor.Services
{
    /// <summary>
    /// Service pour gérer les templates prédéfinis.
    /// </summary>
    public class TemplateService
    {
        public List<TemplateModel> GetAvailableTemplates()
        {
            return new List<TemplateModel>
            {
                GetInvoiceTemplate(),
                GetReportTemplate(),
                GetCertificateTemplate()
            };
        }

        private TemplateModel GetInvoiceTemplate()
        {
            return new TemplateModel
            {
                Id = "invoice",
                Name = "Facture",
                Description = "Template de facture professionnelle avec tableau d'articles",
                Type = TemplateType.DDL,
                Content = @"\document
{
  \section
  {
    \heading1
    {
      Facture N° {{ InvoiceNumber }}
    }
    \paragraph
    {
      Client: {{ CustomerName }}
    }
    \paragraph
    {
      Date: {{ InvoiceDate }}
    }
    \paragraph
    {
      Adresse: {{ CustomerAddress }}
    }
    
    \heading2
    {
      Détails de la facture
    }
    
    \table
    {
      \row
      {
        \cell { Description }
        \cell { Quantité }
        \cell { Prix Unitaire }
        \cell { Total }
      }
      {{ for item in Items }}
      \row
      {
        \cell { {{ item.Description }} }
        \cell { {{ item.Quantity }} }
        \cell { {{ item.UnitPrice }} EUR }
        \cell { {{ item.Total }} EUR }
      }
      {{ end }}
    }
    
    \paragraph
    {
      Montant Total: {{ GrandTotal }} EUR
    }
  }
}",
                SampleData = new Dictionary<string, string>
                {
                    { "InvoiceNumber", "INV-2024-001" },
                    { "CustomerName", "Acme Corporation" },
                    { "InvoiceDate", "30 Avril 2024" },
                    { "CustomerAddress", "123 Business Street, New York, NY 10001" },
                    { "GrandTotal", "2,450.00" }
                },
                SampleTables = new Dictionary<string, List<Dictionary<string, string>>>
                {
                    {
                        "Items",
                        new List<Dictionary<string, string>>
                        {
                            new Dictionary<string, string>
                            {
                                { "Description", "Consulting Services" },
                                { "Quantity", "10" },
                                { "UnitPrice", "150.00" },
                                { "Total", "1,500.00" }
                            },
                            new Dictionary<string, string>
                            {
                                { "Description", "Software License" },
                                { "Quantity", "5" },
                                { "UnitPrice", "190.00" },
                                { "Total", "950.00" }
                            }
                        }
                    }
                }
            };
        }

        private TemplateModel GetReportTemplate()
        {
            return new TemplateModel
            {
                Id = "report",
                Name = "Rapport Mensuel",
                Description = "Template de rapport mensuel avec résumé exécutif",
                Type = TemplateType.DDL,
                Content = @"\document
{
  \section
  {
    \heading1
    {
      Rapport Mensuel - {{ Month }} {{ Year }}
    }
    
    \heading2
    {
      Résumé Exécutif
    }
    \paragraph
    {
      Département: {{ Department }}
    }
    \paragraph
    {
      Responsable: {{ Manager }}
    }
    \paragraph
    {
      Période: {{ StartDate }} à {{ EndDate }}
    }
    
    \heading2
    {
      Indicateurs Clés
    }
    \paragraph
    {
      Objectifs Atteints: {{ ObjectivesAchieved }}%
    }
    \paragraph
    {
      Revenus: {{ Revenue }} EUR
    }
    \paragraph
    {
      Dépenses: {{ Expenses }} EUR
    }
    
    \heading2
    {
      Résultats par Projet
    }
    
    \table
    {
      \row
      {
        \cell { Projet }
        \cell { Statut }
        \cell { Progression }
        \cell { Budget Utilisé }
      }
      {{ for project in Projects }}
      \row
      {
        \cell { {{ project.Name }} }
        \cell { {{ project.Status }} }
        \cell { {{ project.Progress }}% }
        \cell { {{ project.BudgetUsed }} EUR }
      }
      {{ end }}
    }
    
    \paragraph
    {
      Notes: {{ Notes }}
    }
  }
}",
                SampleData = new Dictionary<string, string>
                {
                    { "Month", "Avril" },
                    { "Year", "2024" },
                    { "Department", "Engineering" },
                    { "Manager", "Jean Dupont" },
                    { "StartDate", "01/04/2024" },
                    { "EndDate", "30/04/2024" },
                    { "ObjectivesAchieved", "92" },
                    { "Revenue", "125,000" },
                    { "Expenses", "45,000" },
                    { "Notes", "Tous les objectifs ont été atteints. Excellente performance de l'équipe." }
                },
                SampleTables = new Dictionary<string, List<Dictionary<string, string>>>
                {
                    {
                        "Projects",
                        new List<Dictionary<string, string>>
                        {
                            new Dictionary<string, string>
                            {
                                { "Name", "Platform Migration" },
                                { "Status", "En cours" },
                                { "Progress", "75" },
                                { "BudgetUsed", "28,000" }
                            },
                            new Dictionary<string, string>
                            {
                                { "Name", "API Development" },
                                { "Status", "Complété" },
                                { "Progress", "100" },
                                { "BudgetUsed", "12,000" }
                            }
                        }
                    }
                }
            };
        }

        private TemplateModel GetCertificateTemplate()
        {
            return new TemplateModel
            {
                Id = "certificate",
                Name = "Certificat",
                Description = "Template de certificat d'accomplissement",
                Type = TemplateType.DDL,
                Content = @"\document
{
  \section
  {
    \heading1
    {
      Certificat d'Accomplissement
    }
    
    \paragraph
    {
      Ceci certifie que
    }
    
    \heading2
    {
      {{ RecipientName }}
    }
    
    \paragraph
    {
      a complété avec succès
    }
    
    \heading2
    {
      {{ CourseName }}
    }
    
    \paragraph
    {
      Date de complétion: {{ CompletionDate }}
    }
    
    \paragraph
    {
      Instructeur: {{ InstructorName }}
    }
    
    \paragraph
    {
      Numéro de Certificat: {{ CertificateNumber }}
    }
  }
}",
                SampleData = new Dictionary<string, string>
                {
                    { "RecipientName", "Marie Martin" },
                    { "CourseName", "Advanced Blazor WebAssembly Development" },
                    { "CompletionDate", "30 Avril 2024" },
                    { "InstructorName", "Dr. Jean Leclerc" },
                    { "CertificateNumber", "CERT-2024-0042" }
                }
            };
        }
    }
}
