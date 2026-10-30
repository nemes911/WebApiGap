using System;
using System.Collections.Generic;

namespace API_GAI.DbServices.SRC.Models;

public partial class IncidentOfficer
{
    public Guid Id { get; set; }

    public Guid? IncidentId { get; set; }

    public Guid? OfficerId { get; set; }

    public virtual Officer? Officer { get; set; }
}
