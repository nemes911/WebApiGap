using System;
using System.Collections.Generic;

namespace BlazorApp1.DTO.Request;


public partial class PoliceDepartment
{
    public int Id { get; set; }

    public int DistrictId { get; set; }

    public string ChiefFirstName { get; set; } = null!;

    public string ChiefLastName { get; set; } = null!;

    public string? ChiefMiddleName { get; set; }

    public string Address { get; set; } = null!;

    public virtual District District { get; set; } = null!;
}
