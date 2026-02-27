using System;
using System.Collections.Generic;

namespace BlazorApp1.DTO.Request;


public partial class PoliceStation
{
    public int Id { get; set; }

    public int DistrictId { get; set; }

    public string Address { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public virtual District District { get; set; } = null!;

    public virtual ICollection<Incident> Incidents { get; set; } = new List<Incident>();
}
