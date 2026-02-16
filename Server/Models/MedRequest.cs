namespace PegasusMedApi.Server.Models;

public class MedRequest
{
    public int Id { get; set; }
    public string ClientId { get; set; }
    public string ItemDetails { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<VendorStatus> VendorAssignments { get; set; } = new();
}

public class VendorStatus
{
    public int Id { get; set; }
    public int MedRequestId { get; set; } 

    public string VendorId { get; set; } = string.Empty;
    public bool IsFlagged { get; set; } = false;
    public DateTime? AcknowledgedAt { get; set; }
}