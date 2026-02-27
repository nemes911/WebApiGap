using System;
using System.Collections.Generic;

namespace BlazorApp1.DTO.Request;


public partial class SocialStatus
{
    public int Id { get; set; }

    public string StatusName { get; set; } = null!;

    public virtual ICollection<Person> People { get; set; } = new List<Person>();
}
