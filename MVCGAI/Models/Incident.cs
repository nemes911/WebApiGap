using System;
using System.Collections.Generic;

namespace API_GAI.DbServices.SRC.Models;

public partial class Incident
{
    public Guid Id { get; set; }

    public int? IncidentClassId { get; set; }

    public DateOnly IncidentDate { get; set; }

    public string Description { get; set; } = null!;

    public decimal? RepairCost { get; set; }

    public DateTime Timestamp { get; set; }

    public string Location { get; set; } = null!;

    public int PoliceStationId { get; set; }

    public virtual IncidentClassification? IncidentClass { get; set; }

    public virtual ICollection<IncidentVehicle> IncidentVehicles { get; set; } = new List<IncidentVehicle>();

    public virtual PoliceStation PoliceStation { get; set; } = null!;
}
