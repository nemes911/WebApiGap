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
                        //incidents
                       new() {Name = "date", Label="Дата ДТП", Type = "date"},
                       new() {Name = "location", Label="Место ДТП", Type = "text"},
                       new() {Name = "officer", Label="Сотрудник", Type = "text"},
                       new() {Name ="description", Label="Описание ДТП", Type ="text"},
                       new() {Name="Count vehicle", Label="Количество поврежденных машин", Type="number"},
                       //vehicle A
                       new() {Name="Vehicle A", Label="Транспортное средство А", Type="text"},
                       new() {Name="serialNumber", Label="Гос номер", Type="text"},
                       new() {Name="VIN", Label="VIN", Type="text"},
                       new(){Name="Owner_name", Label="Owner", Type="text"},
                       new(){Name="Owner_lastname", Label="Last_name", Type="text"},
                       new(){ Name="Owner_midle_name", Label="Midle_name", Type="text"},
                       new() {Name="passport_number", Label="Passport", Type="number"},
                       new() {Name="Passport_serial", Label="Passport", Type="number"},
                       new() {Name="Insurance", Label="Comnpany_name", Type="text"},
                       //vehicle B
                       new() {Name="Vehicle B", Label="Транспортное средство B", Type="text"},
                       new() {Name="serialNumber", Label="Гос номер", Type="text"},
                       new() {Name="VIN", Label="VIN", Type="text"},
                       new(){Name="Owner_name", Label="Owner", Type="text"},
                       new(){Name="Owner_lastname", Label="Last_name", Type="text"},
                       new(){ Name="Owner_midle_name", Label="Midle_name", Type="text"},
                       new() {Name="passport_number", Label="Passport", Type="number"},
                       new() {Name="Passport_serial", Label="Passport", Type="number"},
                       new() {Name="Insurance", Label="Comnpany_name", Type="text"},
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
