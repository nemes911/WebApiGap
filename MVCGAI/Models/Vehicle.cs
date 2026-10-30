using System;
using System.Collections.Generic;

namespace API_GAI.DbServices.SRC.Models;

public partial class Vehicle
{
    public Guid Id { get; set; }

    public int SerialNumber { get; set; }

    public string? Color { get; set; }

    public Guid? OwnerId { get; set; }

    public virtual ICollection<IncidentVehicle> IncidentVehicles { get; set; } = new List<IncidentVehicle>();

    public virtual Person? Owner { get; set; }
}
