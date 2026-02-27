using BlazorApp1.Data.Default;
using System.Reflection.Metadata;

namespace BlazorApp1.Data.Dinamic.Service
{
    public class DocumentService
    {
        public Documents GetDocyment(string type)
        {
            return type switch
            {
                "accident" => new Documents
                {
                    DocumentType = "accident",
                    field = new List<FieldDoc>
                   {
                       new() {Name = "date", Label="Дата ДТП", Type = "date"},
                       new() {Name = "Location", Label="Место ДТП", Type = "text"},
                       new() {Name = "officer", Label="Сотрудник", Type = "text"}
                   }
                },

                "license_revocation" => new Documents
                {
                    DocumentType = "license_revocation",
                    field = new List<FieldDoc>
                    {
                        new() {Name="court", Label="Суд", Type="text"},
                        new() {Name="judge", Label="Судья", Type="text"},
                        new() {Name="driver", Label="Водитель", Type="text"}
                    }
                },

                _ => throw new Exception("Unknow document type")
            };
        }
    }
}
