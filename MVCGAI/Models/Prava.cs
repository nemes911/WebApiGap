using API_GAI.DbServices.SRC.Models;

namespace MVCGAI.Models
{
    public partial class Prava
    {
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public string Series { get; set; }

        public int Number {  get; set; }

        public DateTime Date_end { get; set; }

        public string Kod_podrazdeleniya { get; set; }

        public string Type { get; set; }

        public bool Status { get; set; }

        // poka koment public ICollection<Person> People { get; set; }
    }
}
