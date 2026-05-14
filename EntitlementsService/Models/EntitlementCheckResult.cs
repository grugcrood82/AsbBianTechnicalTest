namespace AsbBianTechnicalTest.Models;

public class EntitlementCheckResult
{
    public bool Allowed { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string? GrantedEntitlement { get; set; }
    public List<string> Path { get; set; } = [];
}