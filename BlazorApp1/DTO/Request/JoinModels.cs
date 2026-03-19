namespace BlazorApp1.DTO.Request;

public class JoinPrava
{
    public Prava prava { get; set; } = new();
    public Person Person { get; set; } = new();
    public Vehicle Vehicle { get; set; } = new();

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
    public string OfficerName { get; set; } = string.Empty;
    public string RankName { get; set; } = string.Empty;

    public IncidentOfficerDto(Guid incidentId, DateOnly incidentDate, string officerName, string rankName)
    {
        IncidentId = incidentId;
        IncidentDate = incidentDate;
        OfficerName = officerName;
        RankName = rankName;
    }

    public IncidentOfficerDto() { }
}

