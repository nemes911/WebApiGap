using System;
using System.Collections.Generic;

namespace API_GAI.DbServices.SRC.Models;

public partial class District
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<PoliceDepartment> PoliceDepartments { get; set; } = new List<PoliceDepartment>();

    public virtual ICollection<PoliceStation> PoliceStations { get; set; } = new List<PoliceStation>();
}
