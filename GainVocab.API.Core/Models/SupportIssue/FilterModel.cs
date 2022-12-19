namespace GainVocab.API.Core.Models.SupportIssue
{
    public class FilterModel
    {
        public string? PublicId { get; set; }
        public List<bool>? IsResolved { get; set; }
        public List<string>? TypeId { get; set; }
        public List<string>? ReporterId { get; set; }
        public List<string>? Created { get; set; }
        public List<string>? Updated { get; set; }
    }
}
