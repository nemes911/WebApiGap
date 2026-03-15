using API_GAI.DbServices.SRC.Models;

namespace WebApiGap.DbServices.SRC.Models
{
    public class JoinPrava
    {
        public Prava prava {  get; set; }

        public Person Person { get; set; }

        public Vehicle Vehicle { get; set; }

        public JoinPrava(Prava prava, Person person, Vehicle vehicle)
        {
            this.prava = prava;
            Person = person;
            Vehicle = vehicle;
        }
        public JoinPrava() { }
    }


    public class IncidentOfficerDto
    {
        public Guid IncidentId { get; set; }

        public DateOnly IncidentDate { get; set; }

        public string OfficerName { get; set; }

        public string RankName { get; set; }

        public IncidentOfficerDto(Guid incidentId, DateOnly incidentDate, string officerName, string rankName)
        {
            IncidentId = incidentId;
            IncidentDate = incidentDate;
            OfficerName = officerName;
            RankName = rankName;
        }

        public IncidentOfficerDto() { }
    }
}
