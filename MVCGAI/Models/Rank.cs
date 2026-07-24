using System;
using System.Collections.Generic;

namespace API_GAI.DbServices.SRC.Models;

public partial class Rank
{
    public int Id { get; set; }

    public string? RankName { get; set; }

    public virtual ICollection<Officer> Officers { get; set; } = new List<Officer>();
}
