using System;
using System.Collections.Generic;

namespace API_GAI.DbServices.SRC.Models;

public partial class IncidentVehicle
{
    public Guid Id { get; set; }

    public Guid IncidentId { get; set; }

    public Guid VehicleId { get; set; }

    public virtual Incident Incident { get; set; } = null!;

    public virtual Vehicle Vehicle { get; set; } = null!;
}
