namespace AsbBianTechnicalTest.Models;

public class EntitlementCheckRequest
{
    public string SubjectId { get; set; } = string.Empty;
    public string Permission { get; set; } = string.Empty;
    public string ResourceId { get; set; } = string.Empty;
}