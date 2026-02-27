using System.Drawing;

namespace WebApiGap.DbServices.SRC.Models
{
    public class ViewIncidents
    {
        public Guid incident_id { get; set; }

        public int incident_class_id { get; set; }

        public DateOnly incident_date { get; set; }

        public string description { get; set; }

        public decimal repair_cost { get; set; }

        public Guid vehicle_id { get; set; }

        public int serial_number { get; set; }

        public string color { get; set; }

        public Guid owner_id { get; set; }

        public string car_brand { get; set; }

        public string insurance_company { get; set; }

        public string vin { get; set; }

        public ViewIncidents(Guid incident_id, int incident_class_id, DateOnly incident_date, string description, decimal repair_cost, Guid vehicle_id, int serial_number, string color, Guid owner_id, string car_brand, string insurance_company, string vin)
        {
            this.incident_id = incident_id;
            this.incident_class_id = incident_class_id;
            this.incident_date = incident_date;
            this.description = description;
            this.repair_cost = repair_cost;
            this.vehicle_id = vehicle_id;
            this.serial_number = serial_number;
            this.color = color;
            this.owner_id = owner_id;
            this.car_brand = car_brand;
            this.insurance_company = insurance_company;
            this.vin = vin;
        }

        public ViewIncidents() { }
    }
}
