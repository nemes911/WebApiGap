namespace WebApiGap.DbServices.SRC.DTOModels
{
    public partial class HumansVies
    {
        public Guid person_id { get; set; }

        public string first_name { get; set; } = null!;

        public string last_name { get; set; } = null!;

        public string? middle_name { get; set; }

        public int passport_number { get; set; }

        public int passport_series { get; set; }


        // Social status
        public int social_status_id { get; set; }

        public string social_status_name { get; set; } = null!;


        // Prava
        public Guid? prava_id { get; set; }

        public DateTime? prava_date { get; set; }

        public string? prava_series { get; set; }

        public int? prava_number { get; set; }

        public DateTime? prava_date_end { get; set; }

        public string? kod_podrazdeleniya { get; set; }

        public string[]? prava_type { get; set; }

        public bool? prava_status { get; set; }
    }
}
