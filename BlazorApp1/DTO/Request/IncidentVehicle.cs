using System;
using System.Collections.Generic;

namespace BlazorApp1.DTO.Request;


public partial class IncidentVehicle
{
    public Guid Id { get; set; }

    public Guid IncidentId { get; set; }

    public Guid VehicleId { get; set; }

    public virtual Incident Incident { get; set; } = null!;

    public virtual Vehicle Vehicle { get; set; } = null!;
}
