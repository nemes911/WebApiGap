using System;
using System.Collections.Generic;

namespace BlazorApp1.DTO.Request;


public partial class Vehicle
{
    public Guid Id { get; set; }

    public int SerialNumber { get; set; }

    public string? Color { get; set; }

    public Guid OwnerId { get; set; }

    public string CarBrand { get; set; }

    public string? Insurance_company {  get; set; }

    public string Vin {  get; set; }

    public virtual ICollection<IncidentVehicle> IncidentVehicles { get; set; } = new List<IncidentVehicle>();

    public virtual Person? Owner { get; set; }
}
