namespace BlazorApp1.DTO.Request;

public partial class Prava
{
    public Guid id { get; set; }
    public DateTime date { get; set; }
    public string series { get; set; } = string.Empty;
    public int number { get; set; }
    public DateTime date_end { get; set; }
    public string kod_podrazdeleniya { get; set; } = string.Empty;
    public string[] type { get; set; } = Array.Empty<string>();
    public bool status { get; set; }
}

