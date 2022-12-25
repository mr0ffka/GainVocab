namespace GainVocab.API.Core.Models.Course
{
    public class ActiveListItemModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? LanguageFrom { get; set; }
        public string? LanguageTo { get; set; }
        public string? Description { get; set; }
        public int? PercentProgress { get; set; }
        public int? AmountOfErrors { get; set; }
    }
}
