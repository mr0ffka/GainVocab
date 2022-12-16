namespace GainVocab.API.Core.Models.SupportIssue
{
    public class FilterModel
    {
        public List<bool>? IsResolved { get; set; }
        public List<string>? TypeId { get; set; }
        public List<string>? ReporterId { get; set; }
        public string? CreatedFrom { get; set; }
        public string? CreatedTo { get; set; }
        public string? UpdatedFrom { get; set; }
        public string? UpdatedTo { get; set; }
    }
}
