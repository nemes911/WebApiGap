using System;
using System.Collections.Generic;

namespace API_GAI.DbServices.SRC.Models;

public partial class SocialStatus
{
    public int Id { get; set; }

    public string StatusName { get; set; } = null!;

    public virtual ICollection<Person> People { get; set; } = new List<Person>();
}
