namespace WebApiGap.DbServices.SRC.DTOModels
{
    public class IncidentsByIdDistrict
    {
        public Guid Id { get; set; }

        public int Incident_class_Id { get; set; }

        public DateTime Incident_Date { get; set; } // 

        public string Description { get; set; } //

        public decimal Repair_cost { get; set; } // 

        public DateTime TimeStamp { get; set; } // 

        public string Location { get; set; } // 

        public int PoliceStation_Id { get; set; }

        public IncidentsByIdDistrict(Guid id, int incident_class_Id, DateTime incident_Date, string description, decimal repair_cost, DateTime timeStamp, string location, int policeStation_Id)
        {
            Id = id;
            Incident_class_Id = incident_class_Id;
            Incident_Date = incident_Date;
            Description = description;
            Repair_cost = repair_cost;
            TimeStamp = timeStamp;
            Location = location;
            PoliceStation_Id = policeStation_Id;
        }
        public IncidentsByIdDistrict( ) { }
    }
}
