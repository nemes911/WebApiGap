using System;
using System.Collections.Generic;

namespace API_GAI.DbServices.SRC.Models;

public partial class IncidentClassification
{
    public int Id { get; set; }

    public string ClassificationName { get; set; } = null!;

    public virtual ICollection<Incident> Incidents { get; set; } = new List<Incident>();
}
