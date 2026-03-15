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

public class DistricStat
{
    public string District { get; set; }

    public int TotalCountIncidents { get; set; }

    public DistricStat(string District, int TotalCountIncidents)
    {
        this.District = District;
        this.TotalCountIncidents = TotalCountIncidents;
    }
    public DistricStat() { }
}

public class DistrictCount
{
    public int DistrictId {  get; set; }

    public int? IncidentCount {  get; set; }

    public decimal? TotalDamage { get; set; }

    public DistrictCount(int DistrictId, int? IncidentCount, decimal? TotalDamage)
    {
        this.DistrictId = DistrictId;
        this.IncidentCount = IncidentCount;
        this.TotalDamage = TotalDamage;
    }
    public DistrictCount() { }
}
