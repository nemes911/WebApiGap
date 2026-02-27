using System;
using System.Collections.Generic;

namespace BlazorApp1.DTO.Request;


public partial class Rank
{
    public int Id { get; set; }

    public string? RankName { get; set; }

    public virtual ICollection<Officer> Officers { get; set; } = new List<Officer>();
}
