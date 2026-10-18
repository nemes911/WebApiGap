namespace MVCGAI.DTOModels
{
    public class HumansVies
    {
        public Guid PersonId { get; set; }

        public string? FirstName { get; set; } = null!;

        public string? LastName { get; set; } = null!;

        public string? MiddleName { get; set; }

        public int? PassportNumber { get; set; }

        public int? PassportSeries { get; set; }


        // Social status
        public int SocialStatusId { get; set; }

        public string? SocialStatusName { get; set; } = null!;


        // Prava
        public Guid? PravaId { get; set; }

        public DateTime? PravaDate { get; set; }

        public string? PravaSeries { get; set; }

        public int? PravaNumber { get; set; }

        public DateTime? PravaDateEnd { get; set; }

        public string? KodPodrazdeleniya { get; set; }

        public string[]? PravaType { get; set; }

        public bool? PravaStatus { get; set; }

    }
}
