namespace BlazorApp1.Data.Default
{
    public class Documents
    {
        public string DocumentType { get; set; } = "";

        public List<FieldDoc> field { get; set; } = new();
    }
}
