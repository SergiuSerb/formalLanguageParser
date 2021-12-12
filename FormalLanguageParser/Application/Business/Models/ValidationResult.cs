namespace Application.Business.Models
{
    public struct ValidationResult
    {
        public bool IsValid { get; set; }

        public string Reason { get; set; }
    }
}