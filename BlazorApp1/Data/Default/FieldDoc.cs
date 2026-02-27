namespace BlazorApp1.Data.Default
{
    public class FieldDoc
    {
        public string Name { get; set; } = "";

        public string Label { get; set; } = "";

        public string Type { get; set; } = "text";

        public string? StringValue { get; set; }
        
        public DateTime? DateValue { get; set; }

        public int? NumberValue { get; set; }
    }
}
