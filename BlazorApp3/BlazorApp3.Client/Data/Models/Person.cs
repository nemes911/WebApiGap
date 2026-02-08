using System;
using System.Collections.Generic;

namespace API_GAI.DbServices.SRC.Models;

public partial class Person
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public int PassportNumber { get; set; }

    public int PassportSeries { get; set; }

    public int SocialStatusId { get; set; }

    public string? CarBrand { get; set; }

    public virtual SocialStatus SocialStatus { get; set; } = null!;

    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}
